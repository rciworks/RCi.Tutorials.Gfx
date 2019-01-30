using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <summary>
    /// Shader interface.
    /// </summary>
    public interface IShader<TVertexIn, TVertex>
        where TVertexIn : struct
        where TVertex : struct, IVertex
    {
        /// <summary>
        /// Execute vertex shader.
        /// </summary>
        TVertex VertexShader(in TVertexIn vertex);

        /// <summary>
        /// Execute pixel (fragment) shader.
        /// </summary>
        Vector4F? PixelShader(in TVertex vertex);
    }
}
