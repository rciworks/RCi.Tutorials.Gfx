using RCi.Tutorials.Gfx.Materials;
using Material = RCi.Tutorials.Gfx.Materials.PositionColor.Material;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.PositionColor
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
            var shader = gfxModel.RenderHost.ShaderLibrary.ShaderPositionColor;
            shader.Update(renderContext.MatrixToClip);
            gfxModel.Render(shader, new BufferBinding(gfxModel.Model.Positions, gfxModel.Model.Colors));
            return default;
        }
    }
}