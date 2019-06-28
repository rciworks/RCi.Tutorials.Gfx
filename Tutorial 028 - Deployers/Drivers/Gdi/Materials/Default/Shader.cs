using RCi.Tutorials.Gfx.Drivers.Gdi.Render;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.Default
{
    /// <summary>
    /// <see cref="Gfx.Materials.Default.Material"/> shader.
    /// </summary>
    public class Shader :
        Shader<VsIn, PsIn>
    {
        private static Vector4F ColorWhite { get; } = new Vector4F(1, 1, 1, 1);

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
            vsout = new PsIn(MatrixToClip.Transform(vsin.Position.ToVector4F(1)));
            return true;
        }

        public override bool PixelShader(in PsIn psin, out Vector4F psout)
        {
            psout = ColorWhite;
            return true;
        }
    }
}
