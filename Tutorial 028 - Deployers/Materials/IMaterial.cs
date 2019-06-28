using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Material to use for model rendering.
    /// </summary>
    public interface IMaterial
    {
        /// <inheritdoc cref="Space"/>
        Space Space { get; set; }

        /// <inheritdoc cref="Deployer{TGfxModel,TMaterial,TResult}.Deploy"/>
        TResult Deploy<TGfxModel, TResult>(TGfxModel gfxModel, RenderContext renderContext)
            where TGfxModel : IGfxModel;
    }
}
