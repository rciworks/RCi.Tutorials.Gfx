using RCi.Tutorials.Gfx.Materials;
using Material = RCi.Tutorials.Gfx.Materials.PositionTexture.Material;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.PositionTexture
{
    /// <inheritdoc />
    public class Deployer :
        Deployer<Material>
    {
        /// <inheritdoc />
        private class DeployerBridgeAdapter :
            BridgeAdapter
        {
            /// <inheritdoc />
            protected override Deployer<Material> Deployer { get; } = new Deployer();
        }

        /// <inheritdoc />
        public override object Deploy(GfxModel gfxModel, Material material, RenderContext renderContext)
        {
            var shader = gfxModel.RenderHost.ShaderLibrary.ShaderPositionTexture;
            shader.Update(renderContext.MatrixToClip, gfxModel.Textures[0]);
            gfxModel.Render(shader, new BufferBinding(gfxModel.Model.Positions, gfxModel.Model.TextureCoordinates));
            return default;
        }
    }
}