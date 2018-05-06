using System;
using System.Drawing;
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
        /// Desired buffer size.
        /// </summary>
        protected Size BufferSize { get; private set; }

        /// <summary>
        /// Viewport size. The size into which buffer will be scaled.
        /// </summary>
        protected Size ViewportSize { get; private set; }

        /// <inheritdoc />
        public FpsCounter FpsCounter { get; private set; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected RenderHost(IRenderHostSetup renderHostSetup)
        {
            HostHandle = renderHostSetup.HostHandle;
            HostInput = renderHostSetup.HostInput;

            BufferSize = HostInput.Size;
            ViewportSize = HostInput.Size;

            FpsCounter = new FpsCounter(new TimeSpan(0, 0, 0, 0, 1000));

            HostInput.SizeChanged += HostInputOnSizeChanged;
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            HostInput.SizeChanged -= HostInputOnSizeChanged;

            FpsCounter.Dispose();
            FpsCounter = default;

            BufferSize = default;
            ViewportSize = default;

            HostInput.Dispose();
            HostInput = default;

            HostHandle = default;
        }

        #endregion

        #region // routines

        /// <inheritdoc cref="IInput.SizeChanged" />
        private void HostInputOnSizeChanged(object sender, ISizeEventArgs args)
        {
            var size = args.NewSize;

            // sanity check
            if (size.Width < 1 || size.Height < 1)
            {
                size = new Size(1, 1);
            }

            ResizeBuffers(size);
            ResizeViewport(size);
        }

        /// <summary>
        /// Resize all buffers.
        /// </summary>
        protected virtual void ResizeBuffers(Size size)
        {
            BufferSize = size;
        }

        /// <summary>
        /// Resize viewport.
        /// </summary>
        protected virtual void ResizeViewport(Size size)
        {
            ViewportSize = size;
        }

        #endregion

        #region // render

        /// <inheritdoc />
        public void Render()
        {
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
