using System;
using RCi.Tutorials.Gfx.Common.Camera;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization
{
    public partial class Pipeline<TVsIn, TPsIn>
    {
        /// <summary>
        /// Triangle primitive.
        /// </summary>
        internal struct PrimitiveTriangle
        {
            public TPsIn PsIn0;
            public TPsIn PsIn1;
            public TPsIn PsIn2;
            public Vector4F PositionScreen0;
            public Vector4F PositionScreen1;
            public Vector4F PositionScreen2;
        }

        #region // routines

        private static int TriangleClampX(int value, in Viewport viewport) => value.Clamp(viewport.X, viewport.X + viewport.Width);

        private static int TriangleClampY(int value, in Viewport viewport) => value.Clamp(viewport.Y, viewport.Y + viewport.Height);

        #endregion

        #region // vertex post-processing

        /// <summary>
        /// <see cref="VertexPostProcessing"/> for triangle.
        /// </summary>
        private void VertexPostProcessingTriangle(ref TPsIn psin0, ref TPsIn psin1, ref TPsIn psin2)
        {
            // vertex post processing + primitive assembly
            PrimitiveTriangle primitive;
            primitive.PsIn0 = psin0;
            primitive.PsIn1 = psin1;
            primitive.PsIn2 = psin2;
            VertexPostProcessing(ref primitive.PsIn0, out primitive.PositionScreen0);
            VertexPostProcessing(ref primitive.PsIn1, out primitive.PositionScreen1);
            VertexPostProcessing(ref primitive.PsIn2, out primitive.PositionScreen2);

            // rasterization stage
            RasterizeTriangle(primitive);
        }

        #endregion

        #region // rasterization

        /// <summary>
        /// Rasterize triangle.
        /// </summary>
        private void RasterizeTriangle(in PrimitiveTriangle primitive)
        {
            var vertex0 = primitive.PositionScreen0;
            var vertex1 = primitive.PositionScreen1;
            var vertex2 = primitive.PositionScreen2;

            // sort vertices by y (so that: vertex0.Y < vertex1.Y < vertex2.Y)
            if (vertex1.Y < vertex0.Y)
            {
                U.Swap(ref vertex0, ref vertex1);
            }
            if (vertex2.Y < vertex1.Y)
            {
                U.Swap(ref vertex1, ref vertex2);
            }
            if (vertex1.Y < vertex0.Y)
            {
                U.Swap(ref vertex0, ref vertex1);
            }

            const float error = 0.0001f;
            if (Math.Abs(vertex0.Y - vertex1.Y) < error)
            {
                // natural flat top

                if (vertex1.X < vertex0.X)
                {
                    U.Swap(ref vertex0, ref vertex1);
                }
                /*
                    (v0)--(v1)
                       \  /
                       (v2)
                */
                RasterizeTriangleFlatTop(vertex0, vertex1, vertex2);
            }
            else if (Math.Abs(vertex1.Y - vertex2.Y) < error)
            {
                // natural flat bottom

                if (vertex2.X < vertex1.X)
                {
                    U.Swap(ref vertex1, ref vertex2);
                }
                /*
                       (v0)
                       /  \
                    (v1)--(v2)
                */
                RasterizeTriangleFlatBottom(vertex1, vertex2, vertex0);
            }
            else
            {
                // regular triangle

                // find splitting vertex (and interpolate)
                var alpha = (vertex1.Y - vertex0.Y) / (vertex2.Y - vertex0.Y);
                var interpolant = vertex0.InterpolateLinear(vertex2, alpha);

                if (vertex1.X < interpolant.X)
                {
                    /*
                          (v0)
                          / |
                      (v1)-(i)
                          \ |
                          (v2)
                    */
                    RasterizeTriangleFlatBottom(vertex1, interpolant, vertex0);
                    RasterizeTriangleFlatTop(vertex1, interpolant, vertex2);
                }
                else
                {
                    /*
                        (v0)
                         | \
                        (i)-(v1)
                         | /
                        (v2)
                    */
                    RasterizeTriangleFlatBottom(interpolant, vertex1, vertex0);
                    RasterizeTriangleFlatTop(interpolant, vertex1, vertex2);
                }
            }
        }

        private void RasterizeTriangleFlatTop(Vector4F vertexLeft, Vector4F vertexRight, Vector4F vertexBottom)
        {
            var height = vertexBottom.Y - vertexLeft.Y;
            var deltaLeft = (vertexBottom - vertexLeft) / height;
            var deltaRight = (vertexBottom - vertexRight) / height;
            RasterizeTriangleFlat(vertexLeft, vertexRight, deltaLeft, deltaRight, height);
        }

        private void RasterizeTriangleFlatBottom(Vector4F vertexLeft, Vector4F vertexRight, Vector4F vertexTop)
        {
            var height = vertexLeft.Y - vertexTop.Y;
            var deltaLeft = (vertexLeft - vertexTop) / height;
            var deltaRight = (vertexRight - vertexTop) / height;
            RasterizeTriangleFlat(vertexTop, vertexTop, deltaLeft, deltaRight, height);
        }

        private void RasterizeTriangleFlat(Vector4F edgeLeft, Vector4F edgeRight, Vector4F deltaLeft, Vector4F deltaRight, float height)
        {
            // get where we start and end vertically
            var yStart = TriangleClampY((int)Math.Round(edgeLeft.Y), RenderHost.CameraInfo.Viewport);
            var yEnd = TriangleClampY((int)Math.Round(edgeLeft.Y + height), RenderHost.CameraInfo.Viewport);

            // prestep (compensate for clamping + move to middle of the pixel)
            edgeLeft += deltaLeft * (yStart - edgeLeft.Y + 0.5f);
            edgeRight += deltaRight * (yStart - edgeRight.Y + 0.5f);

            // go vertically down
            for (var y = yStart; y < yEnd; y++)
            {
                // increment (interpolate) edges (going down)
                var eLeft = edgeLeft + deltaLeft * (y - yStart);
                var eRight = edgeRight + deltaRight * (y - yStart);

                // get scanline start and end
                var xStart = TriangleClampX((int)Math.Round(eLeft.X), RenderHost.CameraInfo.Viewport);
                var xEnd = TriangleClampX((int)Math.Round(eRight.X), RenderHost.CameraInfo.Viewport);

                // go horizontally (execute scanline)
                for (var x = xStart; x < xEnd; x++)
                {
                    // pass to pixel shader
                    StagePixelShader(x, y, default);
                }
            }
        }

        #endregion
    }
}
