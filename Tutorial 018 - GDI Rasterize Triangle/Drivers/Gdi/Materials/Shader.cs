using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <inheritdoc />
    public abstract class Shader<TVertex, TVertexShader> :
        IShader<TVertex, TVertexShader>
        where TVertex : struct
        where TVertexShader : struct, IVertexShader
    {
        #region // shaders

        /// <inheritdoc />
        public abstract TVertexShader VertexShader(in TVertex vertex);

        /// <inheritdoc />
        public virtual Vector4F? PixelShader(in TVertexShader vertex)
        {
            // by default let's draw in white
            return new Vector4F(1, 1, 1, 1);
        }

        #endregion
    }
}
