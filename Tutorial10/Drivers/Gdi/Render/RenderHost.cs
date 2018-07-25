using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Engine.Render;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render
{
    /// <summary>
    /// Gdi render host.
    /// </summary>
    public class RenderHost :
        Engine.Render.RenderHost
    {
        #region // storage

        /// <summary>
        /// Graphics retrieved from <see cref="IRenderHost.HostHandle"/>.
        /// </summary>
        private Graphics GraphicsHost { get; set; }

        /// <summary>
        /// Device context of <see cref="GraphicsHost"/>.
        /// </summary>
        private IntPtr GraphicsHostDeviceContext { get; set; }

        /// <summary>
        /// Double buffer wrapper.
        /// </summary>
        private BufferedGraphics BufferedGraphics { get; set; }

        /// <summary>
        /// Back buffer.
        /// </summary>
        private DirectBitmap BackBuffer { get; set; }

        /// <summary>
        /// Font for drawing text with <see cref="System.Drawing"/> objects.
        /// </summary>
        private Font FontConsolas12 { get; set; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RenderHost(IRenderHostSetup renderHostSetup) :
            base(renderHostSetup)
        {
            GraphicsHost = Graphics.FromHwnd(HostHandle);
            GraphicsHostDeviceContext = GraphicsHost.GetHdc();
            CreateSurface(HostInput.Size);
            CreateBuffers(BufferSize);
            FontConsolas12 = new Font("Consolas", 12);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            FontConsolas12.Dispose();
            FontConsolas12 = default;

            DisposeBuffers();
            DisposeSurface();

            GraphicsHost.ReleaseHdc(GraphicsHostDeviceContext);
            GraphicsHostDeviceContext = default;

            GraphicsHost.Dispose();
            GraphicsHost = default;

            base.Dispose();
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        protected override void ResizeHost(Size size)
        {
            base.ResizeHost(size);

            DisposeSurface();
            CreateSurface(size);
        }

        /// <inheritdoc />
        protected override void ResizeBuffers(Size size)
        {
            base.ResizeBuffers(size);

            DisposeBuffers();
            CreateBuffers(size);
        }

        private void CreateBuffers(Size size)
        {
            BackBuffer = new DirectBitmap(size);
        }

        private void DisposeBuffers()
        {
            BackBuffer.Dispose();
            BackBuffer = default;
        }

        private void CreateSurface(Size size)
        {
            BufferedGraphics = BufferedGraphicsManager.Current.Allocate(GraphicsHostDeviceContext, new Rectangle(Point.Empty, size));
            BufferedGraphics.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        private void DisposeSurface()
        {
            BufferedGraphics.Dispose();
            BufferedGraphics = default;
        }

        #endregion

        #region // render

        /// <inheritdoc />
        protected override void RenderInternal()
        {
            var graphics = BackBuffer.Graphics;
            graphics.Clear(Color.Black);

            DrawWorldAxis();
            DrawGeometry();

            graphics.DrawString(FpsCounter.FpsString, FontConsolas12, Brushes.Red, 0, 0);

            // flush and swap buffers
            BufferedGraphics.Graphics.DrawImage(BackBuffer.Bitmap, new RectangleF(PointF.Empty, HostSize), new RectangleF(new PointF(-0.5f, -0.5f), BufferSize), GraphicsUnit.Pixel);
            BufferedGraphics.Render(GraphicsHostDeviceContext);
        }

        private void DrawPolyline(IEnumerable<Point3D> points, Space space, Pen pen)
        {
            switch (space)
            {
                case Space.World:
                    DrawPolylineScreenSpace(CameraInfo.Cache.MatrixViewProjectionViewport.Transform(points), pen);
                    break;

                case Space.View:
                    DrawPolylineScreenSpace(CameraInfo.Cache.MatrixViewport.Transform(points), pen);
                    break;

                case Space.Screen:
                    DrawPolylineScreenSpace(points, pen);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(space), space, null);
            }
        }

        private void DrawPolylineScreenSpace(IEnumerable<Point3D> pointsScreen, Pen pen)
        {
            var from = default(Point3D?);
            foreach (var pointScreen in pointsScreen)
            {
                if (from.HasValue)
                {
                    BackBuffer.Graphics.DrawLine(pen, (float)from.Value.X, (float)from.Value.Y, (float)pointScreen.X, (float)pointScreen.Y);
                }
                from = pointScreen;
            }
        }

        private double GetDeltaTime(TimeSpan periodDuration)
        {
            return GetDeltaTime(FrameStarted, periodDuration);
        }

        public static double GetDeltaTime(DateTime timestamp, TimeSpan periodDuration)
        {
            return (timestamp.Second * 1000 + timestamp.Millisecond) % periodDuration.TotalMilliseconds / periodDuration.TotalMilliseconds;
        }

        private void DrawWorldAxis()
        {
            DrawPolyline(new[] { new Point3D(0, 0, 0), new Point3D(1, 0, 0), }, Space.World, Pens.Red);
            DrawPolyline(new[] { new Point3D(0, 0, 0), new Point3D(0, 1, 0), }, Space.World, Pens.LawnGreen);
            DrawPolyline(new[] { new Point3D(0, 0, 0), new Point3D(0, 0, 1), }, Space.World, Pens.Blue);
        }

        private static readonly Point3D[][] CubePolylines = new[]
        {
            new[]
            {
                new Point3D(0, 0, 0),
                new Point3D(1, 0, 0),
                new Point3D(1, 1, 0),
                new Point3D(0, 1, 0),
                new Point3D(0, 0, 0),
            },
            new[]
            {
                new Point3D(0, 0, 1),
                new Point3D(1, 0, 1),
                new Point3D(1, 1, 1),
                new Point3D(0, 1, 1),
                new Point3D(0, 0, 1),
            },
            new[] { new Point3D(0, 0, 0), new Point3D(0, 0, 1), },
            new[] { new Point3D(1, 0, 0), new Point3D(1, 0, 1), },
            new[] { new Point3D(1, 1, 0), new Point3D(1, 1, 1), },
            new[] { new Point3D(0, 1, 0), new Point3D(0, 1, 1), },
        }.Select(cubePolyline => MatrixEx.Translate(-0.5, -0.5, -0.5).Transform(cubePolyline).ToArray()).ToArray();

        private void DrawGeometry()
        {
            // screen space
            DrawPolyline(new[] { new Point3D(3, 20, 0), new Point3D(140, 20, 0) }, Space.Screen, Pens.Gray);

            // view space
            DrawPolyline(new[] { new Point3D(-0.9, -0.9, 0), new Point3D(0.9, -0.9, 0) }, Space.View, Pens.Gray);


            // world space bigger cube
            var angle = GetDeltaTime(new TimeSpan(0, 0, 0, 5)) * Math.PI * 2;
            var matrixModel =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(new UnitVector3D(1, 0, 0), angle) *
                MatrixEx.Translate(1, 0, 0);

            foreach (var cubePolyline in CubePolylines)
            {
                DrawPolyline(matrixModel.Transform(cubePolyline), Space.World, Pens.White);
            }

            // world space smaller cube
            angle = GetDeltaTime(new TimeSpan(0, 0, 0, 1)) * Math.PI * 2;
            matrixModel =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(new UnitVector3D(0, 1, 0), angle) *
                MatrixEx.Translate(0, 1, 0) *
                matrixModel;

            foreach (var cubePolyline in CubePolylines)
            {
                DrawPolyline(matrixModel.Transform(cubePolyline), Space.World, Pens.Yellow);
            }
        }

        #endregion
    }
}
