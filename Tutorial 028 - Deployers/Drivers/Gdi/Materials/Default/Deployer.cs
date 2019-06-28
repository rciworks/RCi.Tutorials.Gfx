using RCi.Tutorials.Gfx.Materials;
using Material = RCi.Tutorials.Gfx.Materials.Default.Material;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.Default
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
            var shader = gfxModel.RenderHost.ShaderLibrary.ShaderDefault;
            shader.Update(renderContext.MatrixToClip);
            gfxModel.Render(shader, new BufferBinding(gfxModel.Model.Positions));
            return default;
        }
    }
}
