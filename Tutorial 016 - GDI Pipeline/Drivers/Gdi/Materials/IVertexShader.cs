using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <summary>
    /// Internal shader vertex interface.
    /// </summary>
    public interface IVertexShader
    {
        /// <summary>
        /// NDC position (vertex shader output).
        /// </summary>
        Vector4F Position { get; }
    }

    /// <summary>
    /// Access to clone shader vertex.
    /// TODO: temporary
    /// </summary>
    public interface IVertexShader<out TVertexShader> :
        IVertexShader
        where TVertexShader : struct, IVertexShader
    {
        /// <summary>
        /// Clone shader vertex with new NDC position.
        /// </summary>
        TVertexShader CloneWithNewPosition(Vector4F position);
    }
}
