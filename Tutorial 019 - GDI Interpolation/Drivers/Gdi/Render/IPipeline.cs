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
    public interface IPipeline<in TVertexIn> :
        IPipeline
        where TVertexIn : struct
    {
        /// <summary>
        /// Render primitives.
        /// </summary>
        void Render(TVertexIn[] vertices, PrimitiveTopology primitiveTopology);
    }

    /// <summary>
    /// <see cref="IPipeline{TVertex}"/> with known internal shader vertex type.
    /// </summary>
    public interface IPipeline<TvertexIn, TVertex> :
        IPipeline<TvertexIn>
        where TvertexIn : struct
        where TVertex : struct, Materials.IVertex
    {
        /// <summary>
        /// Set current shader.
        /// </summary>
        void SetShader(IShader<TvertexIn, TVertex> shader);
    }
}
