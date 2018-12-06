using MathNet.Numerics.LinearAlgebra;
using RCi.Tutorials.Gfx.Materials.Position;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.Position
{
    public class Shader :
        Shader<Vertex, VertexShader>
    {
        #region // storage

        /// <summary>
        /// Transform from world space to clip space.
        /// </summary>
        private Matrix<double> MatrixWorldViewProjection { get; set; } = MatrixEx.Identity;

        /// <summary>
        /// Color in which primitives are gonna be drawn.
        /// </summary>
        private Vector4F Color { get; set; } = new Vector4F(0, 0, 0, 0);

        #endregion

        #region // routines

        /// <summary>
        /// Update global shader memory.
        /// </summary>
        public void Update(Matrix<double> matrixWorldViewProjection, System.Drawing.Color color)
        {
            MatrixWorldViewProjection = matrixWorldViewProjection;
            Color = color.ToVector4F();
        }

        #endregion

        #region // shaders

        /// <inheritdoc />
        public override VertexShader VertexShader(in Vertex vertex)
        {
            return new VertexShader
            (
                MatrixWorldViewProjection.Transform(vertex.Position.ToVector4F(1))
            );
        }

        /// <inheritdoc />
        public override Vector4F? PixelShader(in VertexShader vertex)
        {
            return Color.W > 0 ? Color : default;
        }

        #endregion
    }
}
