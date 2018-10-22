using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
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

            var t = DateTime.UtcNow.Millisecond / 1000.0;
            Color GetColor(int x, int y) => Color.FromArgb
            (
                byte.MaxValue,
                (byte)((double)x / BufferSize.Width * byte.MaxValue),
                (byte)((double)y / BufferSize.Height * byte.MaxValue),
                (byte)(Math.Sin(t * Math.PI) * byte.MaxValue)
            );

            Parallel.For(0, BackBuffer.Buffer.Length, index =>
            {
                BackBuffer.GetXY(index, out var x, out var y);
                BackBuffer.Buffer[index] = GetColor(x, y).ToArgb();
            });

            // screen space triangle
            DrawPolyline(new[]
            {
                new Point3D(100, 100, 0),
                new Point3D(100, 200, 0),
                new Point3D(300, 200, 0),
                new Point3D(100, 100, 0),
            }, Space.Screen, Pens.White);

            // view space triangle
            DrawPolyline(new[]
            {
                new Point3D(0, 0, 0),
                new Point3D(0, -0.9f, 0),
                new Point3D(0.9F, -0.9f, 0),
                new Point3D(0, 0, 0),
            }, Space.View, Pens.Black);

            TestTransformations();

            graphics.DrawString(FpsCounter.FpsString, FontConsolas12, Brushes.Red, 0, 0);
            graphics.DrawString($"Buffer   = {BufferSize.Width}, {BufferSize.Height}", FontConsolas12, Brushes.Cyan, 0, 16);
            graphics.DrawString($"Viewport = {Viewport.Width}, {Viewport.Height}", FontConsolas12, Brushes.Cyan, 0, 32);

            // flush and swap buffers
            BufferedGraphics.Graphics.DrawImage(BackBuffer.Bitmap, new RectangleF(PointF.Empty, Viewport.Size), new RectangleF(new PointF(-0.5f, -0.5f), BufferSize), GraphicsUnit.Pixel);
            BufferedGraphics.Render(GraphicsHostDeviceContext);
        }

        private void DrawPolyline(IEnumerable<Point3D> points, Space space, Pen pen)
        {
            switch (space)
            {
                case Space.World:
                    throw new NotSupportedException();

                case Space.View:
                    DrawPolylineScreenSpace(MatrixEx.Viewport(Viewport).Transform(points), pen);
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

        private void TestTransformations()
        {
            // raw coordinates for arrow in screen space
            var pointsArrowScreen = new[]
            {
                new Point3D(0, 0, 0),
                new Point3D(40, 0, 0),
                new Point3D(35, 10, 0),
                new Point3D(50, 0, 0),
                new Point3D(35, -10, 0),
                new Point3D(40, 0, 0),
            };

            // raw coordinates for arrow in view space
            var pointsArrowView = new[]
            {
                new Point3D(0, 0, 0),
                new Point3D(0.08, 0, 0),
                new Point3D(0.07, 0.02, 0),
                new Point3D(0.1, 0, 0),
                new Point3D(0.07, -0.02, 0),
                new Point3D(0.08, 0, 0),
            };

            // draw default
            DrawPolyline(pointsArrowScreen, Space.Screen, Pens.Yellow);
            DrawPolyline(pointsArrowView, Space.View, Pens.Cyan);

            // get animation params
            var periodDuration = new TimeSpan(0, 0, 0, 5, 0);
            var utcNow = DateTime.UtcNow;
            var t = (utcNow.Second * 1000 + utcNow.Millisecond) % periodDuration.TotalMilliseconds / periodDuration.TotalMilliseconds;
            var sinT = Math.Sin(t * Math.PI * 2);

            // translate
            DrawPolyline((MatrixEx.Translate(sinT * 40, 0, 0) * MatrixEx.Translate(50, 100, 0)).Transform(pointsArrowScreen), Space.Screen, Pens.White);
            DrawPolyline((MatrixEx.Translate(sinT * 0.1, 0, 0) * MatrixEx.Translate(-0.8, 0, 0)).Transform(pointsArrowView), Space.View, Pens.Black);

            // scale
            DrawPolyline((MatrixEx.Scale(t * 2, t * 2, 1) * MatrixEx.Translate(150, 100, 0)).Transform(pointsArrowScreen), Space.Screen, Pens.White);
            DrawPolyline((MatrixEx.Scale(t * 2, t * 2, 1) * MatrixEx.Translate(-0.6, 0, 0)).Transform(pointsArrowView), Space.View, Pens.Black);

            // rotate
            DrawPolyline((MatrixEx.Rotate(new Vector3D(0, 0, 1), t * Math.PI * 2) * MatrixEx.Translate(300, 100, 0)).Transform(pointsArrowScreen), Space.Screen, Pens.White);
            DrawPolyline((MatrixEx.Rotate(new Vector3D(0, 0, 1), t * Math.PI * 2) * MatrixEx.Translate(-0.2, 0, 0)).Transform(pointsArrowView), Space.View, Pens.Black);

            // rotate * translate
            DrawPolyline((
                MatrixEx.Rotate(new Vector3D(0, 0, 1), t * Math.PI * 2) *
                MatrixEx.Translate(0, sinT * 40, 0) *
                MatrixEx.Translate(400, 100, 0)
            ).Transform(pointsArrowScreen), Space.Screen, Pens.White);
            DrawPolyline((
                MatrixEx.Rotate(new Vector3D(0, 0, 1), t * Math.PI * 2) *
                MatrixEx.Translate(0, sinT * 0.2, 0) *
                MatrixEx.Translate(0, 0, 0)
            ).Transform(pointsArrowView), Space.View, Pens.Black);

            // translate * rotate
            DrawPolyline((
                MatrixEx.Translate(0, sinT * 40, 0) *
                MatrixEx.Rotate(new Vector3D(0, 0, 1), t * Math.PI * 2) *        
                MatrixEx.Translate(500, 100, 0)
            ).Transform(pointsArrowScreen), Space.Screen, Pens.White);
            DrawPolyline((
                MatrixEx.Translate(0, sinT * 0.2, 0) *
                MatrixEx.Rotate(new Vector3D(0, 0, 1), t * Math.PI * 2) *
                MatrixEx.Translate(0.4, 0, 0)
            ).Transform(pointsArrowView), Space.View, Pens.Black);
        }

        #endregion
    }
}
