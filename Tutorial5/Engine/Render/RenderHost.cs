using System;
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
            FpsCounter = new FpsCounter(new TimeSpan(0, 0, 0, 0, 1000));
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            FpsCounter.Dispose();
            FpsCounter = default;

            HostInput.Dispose();
            HostInput = default;

            HostHandle = default;
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
