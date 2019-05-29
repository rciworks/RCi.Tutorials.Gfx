using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <summary>
    /// Internal shader vertex interface.
    /// </summary>
    public interface IPsIn<TPsIn> :
        IInterpolateSingle<TPsIn>
    {
        /// <summary>
        /// Clip space position (vertex shader output).
        /// </summary>
        Vector4F Position { get; }

        /// <summary>
        /// Clone vertex with new position.
        /// </summary>
        TPsIn ReplacePosition(Vector4F position);
    }
}
