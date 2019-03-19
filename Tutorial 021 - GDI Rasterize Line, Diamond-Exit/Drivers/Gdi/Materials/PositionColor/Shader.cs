using RCi.Tutorials.Gfx.Drivers.Gdi.Render;
using RCi.Tutorials.Gfx.Materials;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.PositionColor
{
    /// <summary>
    /// <see cref="ShaderType.Position"/>
    /// </summary>
    public class Shader :
        Shader<VsIn, PsIn>
    {
        private Matrix4D MatrixToClip { get; set; } = Matrix4D.Identity;

        public Shader(RenderHost renderHost) :
            base(renderHost)
        {
        }

        public void Update(in Matrix4D matrixToClip)
        {
            MatrixToClip = matrixToClip;
        }

        public override bool VertexShader(in VsIn vsin, out PsIn vsout)
        {
            vsout = new PsIn
            (
                MatrixToClip.Transform(vsin.Position.ToVector4F(1)),
                vsin.Color
            );
            return true;
        }

        public override bool PixelShader(in PsIn psin, out Vector4F psout)
        {
            if (psin.Color.W <= 0)
            {
                psout = default;
                return false;
            }

            psout = psin.Color;
            return true;
        }
    }
}
