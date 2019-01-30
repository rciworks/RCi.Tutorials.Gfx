using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.Position
{
    public class Shader :
        Shader<Gfx.Materials.Position.Vertex, Vertex>
    {
        #region // storage

        /// <summary>
        /// Transform from given space to clip space.
        /// </summary>
        private Matrix4D MatrixToClip { get; set; } = Matrix4D.Identity;

        /// <summary>
        /// Color in which primitives are gonna be drawn.
        /// </summary>
        private Vector4F Color { get; set; } = new Vector4F(0, 0, 0, 0);

        #endregion

        #region // routines

        /// <summary>
        /// Update global shader memory.
        /// </summary>
        public void Update(in Matrix4D matrixToClip, System.Drawing.Color color)
        {
            MatrixToClip = matrixToClip;
            Color = color.ToVector4F();
        }

        #endregion

        #region // shaders

        /// <inheritdoc />
        public override Vertex VertexShader(in Gfx.Materials.Position.Vertex vertex)
        {
            return new Vertex
            (
                MatrixToClip.Transform(vertex.Position.ToVector4F(1))
            );
        }

        /// <inheritdoc />
        public override Vector4F? PixelShader(in Vertex vertex)
        {
            return Color.W > 0 ? Color : default;
        }

        #endregion
    }
}
