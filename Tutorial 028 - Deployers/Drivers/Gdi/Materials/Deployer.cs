using RCi.Tutorials.Gfx.Materials;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <inheritdoc />
    public abstract class Deployer<TMaterial> :
        Deployer<GfxModel, TMaterial, object>
        where TMaterial : IMaterial
    {
        /// <inheritdoc />
        public abstract class BridgeAdapter :
            Bridge.BridgeAdapter<Deployer<GfxModel, TMaterial, object>>
        {
            /// <inheritdoc />
            public override Deployer<GfxModel, TMaterial, object> BridgeValue => Deployer;

            /// <summary>
            /// Particular deployer for known material.
            /// </summary>
            protected abstract Deployer<TMaterial> Deployer { get; }
        }
    }
}
