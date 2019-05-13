using RCi.Tutorials.Gfx.Drivers.Gdi.Render;
using RCi.Tutorials.Gfx.Materials;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.PositionTexture
{
    /// <inheritdoc cref="GfxModel{VsIn,PsIn}"/>
    public class GfxModel :
        GfxModel<VsIn, PsIn>
    {
        /// <inheritdoc cref="IShader"/>
        private Shader Shader { get; }

        /// <summary />
        public GfxModel(RenderHost renderHost, IModel model) :
            base(renderHost, model, renderHost.ShaderLibrary.ShaderPositionTexture, new BufferBinding(model.Positions, model.TextureCoordinates))
        {
            Shader = renderHost.ShaderLibrary.ShaderPositionTexture;
        }

        /// <inheritdoc />
        protected override void ShaderUpdate(in Matrix4D matrixToClip)
        {
            Shader.Update(matrixToClip, RenderHost.TextureLibrary.GetTexture(Model.TextureResource));
        }
    }
}
