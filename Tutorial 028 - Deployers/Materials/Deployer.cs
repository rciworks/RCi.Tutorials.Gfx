namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Deploys model with material.
    /// </summary>
    public abstract class Deployer<TGfxModel, TMaterial, TResult>
        where TGfxModel : IGfxModel
        where TMaterial : IMaterial
    {
        /// <summary>
        /// Deploy model with material to a driver. Driver is inferred from gfx model.
        /// </summary>
        public abstract TResult Deploy(TGfxModel gfxModel, TMaterial material, RenderContext renderContext);
    }
}
