using RCi.Tutorials.Gfx.Materials;
using Material = RCi.Tutorials.Gfx.Materials.Position.Material;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.Position
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
            var shader = gfxModel.RenderHost.ShaderLibrary.ShaderPosition;
            shader.Update(renderContext.MatrixToClip, material.Color);
            gfxModel.Render(shader, new BufferBinding(gfxModel.Model.Positions));
            return default;
        }
    }
}