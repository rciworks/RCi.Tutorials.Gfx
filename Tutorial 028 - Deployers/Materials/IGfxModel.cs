using System;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// <see cref="IModel"/> for particular driver.
    /// </summary>
    public interface IGfxModel :
        IDisposable
    {
        /// <summary>
        /// Render to mounted surface.
        /// </summary>
        void Render(IMaterial material, RenderContext renderContext);
    }
}
