using RCi.Tutorials.Gfx.Drivers.Gdi.Render;
using RCi.Tutorials.Gfx.Materials;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.Position
{
    /// <inheritdoc cref="GfxModel{VsIn,PsIn}"/>
    public class GfxModel :
        GfxModel<VsIn, PsIn>
    {
        /// <inheritdoc cref="IShader"/>
        private Shader Shader { get; }

        /// <summary />
        public GfxModel(RenderHost renderHost, IModel model) :
            base(renderHost, model, renderHost.ShaderLibrary.ShaderPosition, new BufferBinding(model.Positions))
        {
            Shader = renderHost.ShaderLibrary.ShaderPosition;
        }

        /// <inheritdoc />
        protected override void ShaderUpdate(in Matrix4D matrixToClip)
        {
            Shader.Update(matrixToClip, Model.Color);
        }
    }
}
