using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using RCi.Tutorials.Gfx.Drivers.Gdi.Materials;
using RCi.Tutorials.Gfx.Engine.Render;
using RCi.Tutorials.Gfx.Materials;
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
        public DirectBitmap BackBuffer { get; private set; }

        /// <summary>
        /// Shader library.
        /// </summary>
        public ShaderLibrary ShaderLibrary { get; private set; }

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
            ShaderLibrary = new ShaderLibrary();
            FontConsolas12 = new Font("Consolas", 12);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            FontConsolas12.Dispose();
            FontConsolas12 = default;

            ShaderLibrary = default;

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
        protected override void RenderInternal(IEnumerable<IPrimitive> primitives)
        {
            // clear buffers
            BackBuffer.Clear(Color.Black);

            // render primitives
            RenderPrimitives(primitives);

            // draw fps
            BackBuffer.Graphics.DrawString(FpsCounter.FpsString, FontConsolas12, Brushes.Red, 0, 0);

            // flush and swap buffers
            BufferedGraphics.Graphics.DrawImage(
                BackBuffer.Bitmap,
                new RectangleF(PointF.Empty, HostSize),
                new RectangleF(new PointF(-0.5f, -0.5f), BufferSize),
                GraphicsUnit.Pixel);
            BufferedGraphics.Render(GraphicsHostDeviceContext);
        }

        /// <summary>
        /// Draw primitives.
        /// </summary>
        private void RenderPrimitives(IEnumerable<IPrimitive> primitives)
        {
            // TODO: currently we know how to draw only certain type of primitives, so just filter them out
            // TODO: in a future we're gonna solve this generically (without typecasting)
            foreach (var primitive in primitives.OfType<Gfx.Materials.Position.IPrimitive>())
            {
                var pipeline = Pipeline<Gfx.Materials.Position.Vertex, Materials.Position.VertexShader>.Instance;
                pipeline.SetRenderHost(this);
                ShaderLibrary.ShaderPosition.Update(GetMatrixForVertexShader(this, primitive.PrimitiveBehaviour.Space), primitive.Material.Color);
                pipeline.SetShader(ShaderLibrary.ShaderPosition);
                pipeline.Render(primitive.Vertices, primitive.PrimitiveTopology);
            }
        }

        /// <summary>
        /// Get default matrix for vertex shader.
        /// </summary>
        private static Matrix<double> GetMatrixForVertexShader(IRenderHost renderHost, Space space)
        {
            switch (space)
            {
                case Space.World:
                    return renderHost.CameraInfo.Cache.MatrixViewProjection;

                case Space.View:
                    return MatrixEx.Identity;

                case Space.Screen:
                    return renderHost.CameraInfo.Cache.MatrixViewportInverse;

                default:
                    throw new ArgumentOutOfRangeException(nameof(space), space, default);
            }
        }

        #endregion
    }
}
