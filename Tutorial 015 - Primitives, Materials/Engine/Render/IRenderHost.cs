using System;
using System.Collections.Generic;
using System.Drawing;
using RCi.Tutorials.Gfx.Common.Camera;
using RCi.Tutorials.Gfx.Inputs;
using RCi.Tutorials.Gfx.Materials;

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
        /// Desired surface size.
        /// </summary>
        Size HostSize { get; }

        /// <inheritdoc cref="ICameraInfo"/>
        ICameraInfo CameraInfo { get; set; }

        /// <inheritdoc cref="Engine.Render.FpsCounter"/>
        FpsCounter FpsCounter { get; }

        /// <summary>
        /// Render.
        /// </summary>
        void Render(IEnumerable<IPrimitive> primitives);

        /// <summary>
        /// Fires when <see cref="CameraInfo"/> changed.
        /// </summary>
        event EventHandler<ICameraInfo> CameraInfoChanged;
    }
}
