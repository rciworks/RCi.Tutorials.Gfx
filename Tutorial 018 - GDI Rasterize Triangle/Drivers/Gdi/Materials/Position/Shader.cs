using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.Position
{
    public class Shader :
        Shader<Gfx.Materials.Position.Vertex, Vertex>
    {
        #region // storage

        /// <summary>
        /// Transform from world space to clip space.
        /// </summary>
        private Matrix4D MatrixWorldViewProjection { get; set; } = Matrix4D.Identity;

        /// <summary>
        /// Color in which primitives are gonna be drawn.
        /// </summary>
        private Vector4F Color { get; set; } = new Vector4F(0, 0, 0, 0);

        #endregion

        #region // routines

        /// <summary>
        /// Update global shader memory.
        /// </summary>
        public void Update(in Matrix4D matrixWorldViewProjection, System.Drawing.Color color)
        {
            MatrixWorldViewProjection = matrixWorldViewProjection;
            Color = color.ToVector4F();
        }

        #endregion

        #region // shaders

        /// <inheritdoc />
        public override Vertex VertexShader(in Gfx.Materials.Position.Vertex vertex)
        {
            return new Vertex
            (
                MatrixWorldViewProjection.Transform(vertex.Position.ToVector4F(1))
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
