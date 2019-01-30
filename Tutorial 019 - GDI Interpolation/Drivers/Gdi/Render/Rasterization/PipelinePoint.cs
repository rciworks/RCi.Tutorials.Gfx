using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization
{
    public partial class Pipeline<TVsIn, TPsIn>
    {
        /// <summary>
        /// Point primitive.
        /// </summary>
        internal struct PrimitivePoint
        {
            public TPsIn PsIn0;
            public Vector4F PositionScreen0;
        }

        /// <summary>
        /// <see cref="VertexPostProcessing"/> for point.
        /// </summary>
        private void VertexPostProcessingPoint(ref TPsIn psin)
        {
            // vertex post processing + primitive assembly
            PrimitivePoint primitive;
            primitive.PsIn0 = psin;
            VertexPostProcessing(ref primitive.PsIn0, out primitive.PositionScreen0);

            // rasterization stage
            RasterizePoint(primitive);
        }

        /// <summary>
        /// Rasterize point.
        /// </summary>
        private void RasterizePoint(in PrimitivePoint primitive)
        {
            var x = (int)primitive.PositionScreen0.X;
            var y = (int)primitive.PositionScreen0.Y;

            StagePixelShader(x, y, default);
        }
    }
}
