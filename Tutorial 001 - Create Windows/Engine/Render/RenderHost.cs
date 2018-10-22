using System;

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

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected RenderHost(IntPtr hostHandle)
        {
            HostHandle = hostHandle;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            HostHandle = default;
        }

        #endregion
    }
}
