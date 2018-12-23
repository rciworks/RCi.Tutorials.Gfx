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

    /// <summary>
    /// Access to clone shader vertex.
    /// TODO: temporary
    /// </summary>
    public interface IVertex<out TVertex> :
        IVertex
        where TVertex : struct, IVertex
    {
        /// <summary>
        /// Clone shader vertex with new NDC position.
        /// </summary>
        TVertex CloneWithNewPosition(Vector4F position);
    }
}
