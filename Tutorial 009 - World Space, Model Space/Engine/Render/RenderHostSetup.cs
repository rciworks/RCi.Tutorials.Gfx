using System;
using RCi.Tutorials.Gfx.Inputs;

namespace RCi.Tutorials.Gfx.Engine.Render
{
    /// <inheritdoc />
    public class RenderHostSetup :
        IRenderHostSetup
    {
        #region // storage

        /// <inheritdoc />
        public IntPtr HostHandle { get; }

        /// <inheritdoc />
        public IInput HostInput { get; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public RenderHostSetup(IntPtr hostHandle, IInput hostInput)
        {
            HostHandle = hostHandle;
            HostInput = hostInput;
        }

        #endregion
    }
}
