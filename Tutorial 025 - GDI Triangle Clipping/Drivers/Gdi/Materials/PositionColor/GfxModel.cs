using RCi.Tutorials.Gfx.Drivers.Gdi.Render;
using RCi.Tutorials.Gfx.Materials;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.PositionColor
{
    /// <inheritdoc cref="GfxModel{VsIn,PsIn}"/>
    public class GfxModel :
        GfxModel<VsIn, PsIn>
    {
        /// <inheritdoc cref="IShader"/>
        private Shader Shader { get; }

        /// <summary />
        public GfxModel(RenderHost renderHost, IModel model) :
            base(model, renderHost.ShaderLibrary.ShaderPositionColor, new BufferBinding(model.Positions, model.Colors))
        {
            Shader = renderHost.ShaderLibrary.ShaderPositionColor;
        }

        /// <inheritdoc />
        protected override void ShaderUpdate(in Matrix4D matrixToClip)
        {
            Shader.Update(matrixToClip);
        }
    }
}
