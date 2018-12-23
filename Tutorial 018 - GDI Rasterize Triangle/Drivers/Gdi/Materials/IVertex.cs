using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <summary>
    /// Internal shader vertex interface.
    /// </summary>
    public interface IVertex
    {
        /// <summary>
        /// Clip space position (vertex shader output).
        /// </summary>
        Vector4F Position { get; }
    }
}
