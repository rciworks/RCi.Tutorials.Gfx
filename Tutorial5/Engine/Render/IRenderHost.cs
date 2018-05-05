using System;
using RCi.Tutorials.Gfx.Inputs;

namespace RCi.Tutorials.Gfx.Engine.Render
{
    /// <summary>
    /// Interface for render host.
    /// </summary>
    public interface IRenderHost :
        IDisposable
    {
        /// <summary>
        /// Handle of hosting window.
        /// </summary>
        IntPtr HostHandle { get; }

        /// <summary>
        /// Input from host.
        /// </summary>
        IInput HostInput { get; }

        /// <summary>
        /// Measures fps.
        /// </summary>
        FpsCounter FpsCounter { get; }

        /// <summary>
        /// Render.
        /// </summary>
        void Render();
    }
}
