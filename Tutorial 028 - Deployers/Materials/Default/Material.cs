using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Materials.Default
{
    /// <inheritdoc />
    public class Material :
        Materials.Material
    {
        /// <inheritdoc />
        public Material(Space space) :
            base(space)
        {
        }

        /// <inheritdoc />
        public override TResult Deploy<TGfxModel, TResult>(TGfxModel gfxModel, RenderContext renderContext)
        {
            return Deploy<TGfxModel, Material, TResult>(gfxModel, this, renderContext);
        }
    }
}
