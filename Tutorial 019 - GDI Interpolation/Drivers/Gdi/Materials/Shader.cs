using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <inheritdoc />
    public abstract class Shader<TVertexIn, TVertex> :
        IShader<TVertexIn, TVertex>
        where TVertexIn : struct
        where TVertex : struct, IVertex
    {
        #region // shaders

        /// <inheritdoc />
        public abstract TVertex VertexShader(in TVertexIn vertex);

        /// <inheritdoc />
        public virtual Vector4F? PixelShader(in TVertex vertex)
        {
            // by default let's draw in white
            return new Vector4F(1, 1, 1, 1);
        }

        #endregion
    }
}
