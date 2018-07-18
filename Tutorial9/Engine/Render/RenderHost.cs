using System;
using System.Drawing;
using RCi.Tutorials.Gfx.Engine.Common;
using RCi.Tutorials.Gfx.Inputs;

namespace RCi.Tutorials.Gfx.Engine.Render
{
    /// <summary>
    /// Base class for render host.
    /// </summary>
    public abstract class RenderHost :
        IRenderHost
    {
        #region // storage

        /// <inheritdoc />
        public IntPtr HostHandle { get; private set; }

        /// <inheritdoc />
        public IInput HostInput { get; private set; }

        /// <summary>
        /// Desired surface size.
        /// </summary>
        protected Size HostSize { get; private set; }

        /// <summary>
        /// Desired buffer size.
        /// </summary>
        protected Size BufferSize { get; private set; }

        /// <summary>
        /// Viewport.
        /// </summary>
        protected Viewport Viewport { get; private set; }

        /// <inheritdoc />
        public FpsCounter FpsCounter { get; private set; }

        /// <summary>
        /// Timestamp when frame was started (UTC).
        /// </summary>
        protected DateTime FrameStarted { get; private set; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected RenderHost(IRenderHostSetup renderHostSetup)
        {
            HostHandle = renderHostSetup.HostHandle;
            HostInput = renderHostSetup.HostInput;

            HostSize = HostInput.Size;
            BufferSize = HostInput.Size;
            Viewport = new Viewport(Point.Empty, HostSize, 0, 1);

            FpsCounter = new FpsCounter(new TimeSpan(0, 0, 0, 0, 1000));

            HostInput.SizeChanged += HostInputOnSizeChanged;
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            HostInput.SizeChanged -= HostInputOnSizeChanged;

            FpsCounter.Dispose();
            FpsCounter = default;

            Viewport = default;
            BufferSize = default;
            HostSize = default;

            HostInput.Dispose();
            HostInput = default;

            HostHandle = default;
        }

        #endregion

        #region // routines

        /// <inheritdoc cref="IInput.SizeChanged" />
        private void HostInputOnSizeChanged(object sender, ISizeEventArgs args)
        {
            Size Sanitize(Size size)
            {
                if (size.Width < 1 || size.Height < 1)
                {
                    size = new Size(1, 1);
                }
                return size;
            }

            var hostSize = Sanitize(HostInput.Size);
            if (HostSize != hostSize)
            {
                ResizeHost(hostSize);
            }

            var bufferSize = Sanitize(args.NewSize);
            if (BufferSize != bufferSize)
            {
                ResizeBuffers(bufferSize);
            }
        }

        /// <summary>
        /// Resize host.
        /// </summary>
        protected virtual void ResizeHost(Size size)
        {
            HostSize = size;
            Viewport = new Viewport(Point.Empty, size, 0, 1);
        }

        /// <summary>
        /// Resize all buffers.
        /// </summary>
        protected virtual void ResizeBuffers(Size size)
        {
            BufferSize = size;
        }

        #endregion

        #region // render

        /// <inheritdoc />
        public void Render()
        {
            FrameStarted = DateTime.UtcNow;
            FpsCounter.StartFrame();
            RenderInternal();
            FpsCounter.StopFrame();
        }

        /// <summary>
        /// Internal rendering for particular driver.
        /// </summary>
        protected abstract void RenderInternal();

        #endregion
    }
}
