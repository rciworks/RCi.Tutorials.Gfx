using System;
using RCi.Tutorials.Gfx.Inputs;

namespace RCi.Tutorials.Gfx.Engine.Render
{
    /// <summary>
    /// Provides data required for <see cref="IRenderHost"/> creation.
    /// </summary>
    public interface IRenderHostSetup
    {
        /// <inheritdoc cref="IRenderHost.HostHandle" />
        IntPtr HostHandle { get; }

        /// <inheritdoc cref="IRenderHost.HostInput" />
        IInput HostInput { get; }
    }
}
