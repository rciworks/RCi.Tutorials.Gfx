using System;

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
        /// Measures fps.
        /// </summary>
        FpsCounter FpsCounter { get; }

        /// <summary>
        /// Render.
        /// </summary>
        void Render();
    }
}
