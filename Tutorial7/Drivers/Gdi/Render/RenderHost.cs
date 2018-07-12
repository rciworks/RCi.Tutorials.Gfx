using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using RCi.Tutorials.Gfx.Engine.Render;
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

            graphics.DrawString(FpsCounter.FpsString, FontConsolas12, Brushes.Red, 0, 0);
            graphics.DrawString($"Buffer   = {BufferSize.Width}, {BufferSize.Height}", FontConsolas12, Brushes.Cyan, 0, 16);
            graphics.DrawString($"Viewport = {Viewport.Width}, {Viewport.Height}", FontConsolas12, Brushes.Cyan, 0, 32);


            // flush and swap buffers
            BufferedGraphics.Graphics.DrawImage(BackBuffer.Bitmap, new RectangleF(PointF.Empty, Viewport.Size), new RectangleF(new PointF(-0.5f, -0.5f), BufferSize), GraphicsUnit.Pixel);
            BufferedGraphics.Render(GraphicsHostDeviceContext);
        }

        #endregion
    }
}
