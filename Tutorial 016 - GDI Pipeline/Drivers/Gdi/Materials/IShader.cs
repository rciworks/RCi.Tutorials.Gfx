using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <summary>
    /// Shader interface.
    /// </summary>
    public interface IShader<TVertex, TVertexShader>
        where TVertex : struct
        where TVertexShader : struct, IVertexShader
    {
        /// <summary>
        /// Execute vertex shader.
        /// </summary>
        TVertexShader VertexShader(in TVertex vertex);

        /// <summary>
        /// Execute pixel (fragment) shader.
        /// </summary>
        Vector4F? PixelShader(in TVertexShader vertex);
    }
}
