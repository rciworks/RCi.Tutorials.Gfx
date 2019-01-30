using RCi.Tutorials.Gfx.Drivers.Gdi.Render;
using RCi.Tutorials.Gfx.Materials;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.Position
{
    /// <summary>
    /// <see cref="ShaderType.Position"/>
    /// </summary>
    public class Shader :
        Shader<VsIn, PsIn>
    {
        private Matrix4D MatrixToClip { get; set; } = Matrix4D.Identity;
        private Vector4F Color { get; set; } = new Vector4F(0, 0, 0, 0);

        public Shader(RenderHost renderHost) :
            base(renderHost)
        {
        }

        public void Update(in Matrix4D matrixToClip, int color)
        {
            MatrixToClip = matrixToClip;
            Color = color.FromRgbaToVector4F();
        }

        public override bool VertexShader(in VsIn vsin, out PsIn vsout)
        {
            vsout = new PsIn(MatrixToClip.Transform(vsin.Position.ToVector4F(1)));
            return true;
        }

        public override bool PixelShader(in PsIn psin, out Vector4F psout)
        {
            if (Color.W <= 0)
            {
                psout = default;
                return false;
            }

            psout = Color;
            return true;
        }
    }
}
