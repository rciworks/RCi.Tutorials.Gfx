using System;

namespace RCi.Tutorials.Gfx.Engine.Render
{
    public abstract class RenderHost :
        IRenderHost
    {
        #region // storage

        public IntPtr HostHandle { get; private set; }

        #endregion

        #region // ctor

        protected RenderHost(IntPtr hostHandle)
        {
            HostHandle = hostHandle;
        }

        public void Dispose()
        {
            HostHandle = default;
        }

        #endregion
    }
}
