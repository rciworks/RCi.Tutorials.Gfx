using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <inheritdoc cref="IMaterial"/>
    public abstract class Material :
        IMaterial
    {
        /// <summary>
        /// Default <see cref="IMaterial"/>.
        /// </summary>
        public static Default.Material Default { get; } = new Default.Material(Space.View);

        /// <summary>
        /// Deploy with <see cref="Default"/>.
        /// </summary>
        public static TResult DeployDefault<TGfxModel, TResult>(TGfxModel gfxModel, RenderContext renderContext)
            where TGfxModel : IGfxModel
        {
            var deployerDefault = Bridge.Bridge<Deployer<TGfxModel, Default.Material, TResult>>.Value;
            return deployerDefault is null ? default : deployerDefault.Deploy(gfxModel, Default, renderContext);
        }

        /// <summary>
        /// Try to deploy with provided material, fallback to default material.
        /// </summary>
        public static TResult Deploy<TGfxModel, TMaterial, TResult>(TGfxModel gfxModel, TMaterial material, RenderContext renderContext)
            where TGfxModel : IGfxModel
            where TMaterial : IMaterial
        {
            var deployer = Bridge.Bridge<Deployer<TGfxModel, TMaterial, TResult>>.Value;
            return deployer is null
                ? DeployDefault<TGfxModel, TResult>(gfxModel, renderContext)
                : deployer.Deploy(gfxModel, material, renderContext);
        }

        /// <inheritdoc />
        public Space Space { get; set; }

        /// <inheritdoc />
        protected Material(Space space)
        {
            Space = space;
        }

        /// <inheritdoc />
        public virtual TResult Deploy<TGfxModel, TResult>(TGfxModel gfxModel, RenderContext renderContext)
            where TGfxModel : IGfxModel
        {
            return DeployDefault<TGfxModel, TResult>(gfxModel, renderContext);
        }
    }
}
