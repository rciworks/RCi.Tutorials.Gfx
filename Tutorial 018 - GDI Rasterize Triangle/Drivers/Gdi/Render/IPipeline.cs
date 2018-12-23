using RCi.Tutorials.Gfx.Drivers.Gdi.Materials;
using RCi.Tutorials.Gfx.Materials;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render
{
    /// <summary>
    /// Graphics pipeline interface.
    /// </summary>
    public interface IPipeline
    {
        /// <summary>
        /// Set current render host for pipeline.
        /// </summary>
        void SetRenderHost(RenderHost renderHost);
    }

    /// <summary>
    /// <see cref="IPipeline"/> with known vertex type.
    /// </summary>
    public interface IPipeline<in TVertex> :
        IPipeline
        where TVertex : struct
    {
        /// <summary>
        /// Render primitives.
        /// </summary>
        void Render(TVertex[] vertices, PrimitiveTopology primitiveTopology);
    }

    /// <summary>
    /// <see cref="IPipeline{TVertex}"/> with known internal shader vertex type.
    /// </summary>
    public interface IPipeline<TVertex, TVertexShader> :
        IPipeline<TVertex>
        where TVertex : struct
        where TVertexShader : struct, IVertexShader
    {
        /// <summary>
        /// Set current shader.
        /// </summary>
        void SetShader(IShader<TVertex, TVertexShader> shader);
    }
}
