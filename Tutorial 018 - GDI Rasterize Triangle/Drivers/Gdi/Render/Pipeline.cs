using System;
using System.Collections.Generic;
using RCi.Tutorials.Gfx.Drivers.Gdi.Materials;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;
using PrimitiveTopology = RCi.Tutorials.Gfx.Materials.PrimitiveTopology;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render
{
    /// <inheritdoc />
    public class Pipeline<TVertexIn, TVertex> :
        IPipeline<TVertexIn, TVertex>
        where TVertexIn : struct
        where TVertex : struct, IVertex
    {
        #region // singleton

        /// <summary>
        /// Singleton access.
        /// </summary>
        public static IPipeline<TVertexIn, TVertex> Instance { get; } = new Pipeline<TVertexIn, TVertex>();

        #endregion

        #region // storage

        /// <summary>
        /// Ongoing render host.
        /// </summary>
        private RenderHost RenderHost { get; set; }

        /// <summary>
        /// Ongoing shader.
        /// </summary>
        private IShader<TVertexIn, TVertex> Shader { get; set; }

        #endregion

        #region // public access

        /// <inheritdoc />
        public void SetRenderHost(RenderHost renderHost) => RenderHost = renderHost;

        /// <inheritdoc />
        public void SetShader(IShader<TVertexIn, TVertex> shader) => Shader = shader;

        /// <inheritdoc />
        public void Render(TVertexIn[] vertices, PrimitiveTopology primitiveTopology)
        {
            StageVertexShader(vertices, primitiveTopology);
        }

        #endregion

        #region // stages

        /// <summary>
        /// Vertex shader stage.
        /// </summary>
        private void StageVertexShader(TVertexIn[] vertices, PrimitiveTopology primitiveTopology)
        {
            var verticesVsOut = new TVertex[vertices.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                verticesVsOut[i] = Shader.VertexShader(vertices[i]);
            }

            StageVertexPostProcessing(verticesVsOut, primitiveTopology);
        }

        /// <summary>
        /// Vertex post-processing stage.
        /// </summary>
        private void StageVertexPostProcessing(TVertex[] vertices, PrimitiveTopology primitiveTopology)
        {
            switch (primitiveTopology)
            {
                case PrimitiveTopology.PointList:
                    for (var i = 0; i < vertices.Length; i++)
                    {
                        VertexPostProcessingPoint(vertices[i]);
                    }
                    break;

                case PrimitiveTopology.LineList:
                    for (var i = 0; i < vertices.Length; i += 2)
                    {
                        VertexPostProcessingLine(vertices[i], vertices[i + 1]);
                    }
                    break;

                case PrimitiveTopology.LineStrip:
                    for (var i = 0; i < vertices.Length - 1; i++)
                    {
                        VertexPostProcessingLine(vertices[i], vertices[i + 1]);
                    }
                    break;

                case PrimitiveTopology.TriangleList:
                    // TODO:
                    break;

                case PrimitiveTopology.TriangleStrip:
                    // TODO:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(primitiveTopology), primitiveTopology, null);
            }
        }

        /// <summary>
        /// Pixel (fragment) shader stage.
        /// </summary>
        private void StagePixelShader(int x, int y, in TVertex vertex)
        {
            var color = Shader.PixelShader(vertex);

            // check if was discarded
            if (!color.HasValue)
            {
                return;
            }

            StageOutputMerger(x, y, color.Value);
        }

        /// <summary>
        /// Output merger stage.
        /// </summary>
        private void StageOutputMerger(int x, int y, Vector4F color)
        {
            var backBuffer = RenderHost.BackBuffer;
            var index = backBuffer.GetIndex(x, y);

            // sanity check
            if (index < 0 || index >= backBuffer.Buffer.Length)
            {
                return;
            }

            // write color
            backBuffer.Buffer[index] = color.ToArgb();
        }

        #endregion

        #region // vertex post-processing

        #region // primitives

        private struct PrimitivePoint
        {
            public Vector4F PositionScreen0;
        }

        private struct PrimitiveLine
        {
            public Vector4F PositionScreen0;
            public Vector4F PositionScreen1;
        }

        #endregion

        /// <summary>
        /// Post process vertex.
        /// </summary>
        private void VertexPostProcessing(in TVertex vertex, out Vector4F positionScreen)
        {
            // clip space to NDC to screen space
            positionScreen = RenderHost.CameraInfo.Cache.MatrixViewport
                // TODO: currently do perspective division here
                .Transform(vertex.Position)
                .ToVector3FNormalized()
                .ToVector4F(1);
        }

        /// <summary>
        /// Post process point vertex.
        /// </summary>
        private void VertexPostProcessingPoint(in TVertex vertex0)
        {
            // vertex post processing + primitive assembly
            PrimitivePoint primitive;
            VertexPostProcessing(vertex0, out primitive.PositionScreen0);

            // rasterization stage
            RasterizePoint(primitive);
        }

        /// <summary>
        /// Post process line vertices.
        /// </summary>
        private void VertexPostProcessingLine(in TVertex vertex0, in TVertex vertex1)
        {
            // vertex post processing + primitive assembly
            PrimitiveLine primitive;
            VertexPostProcessing(vertex0, out primitive.PositionScreen0);
            VertexPostProcessing(vertex1, out primitive.PositionScreen1);

            // rasterization stage
            RasterizeLine(primitive);
        }

        #endregion

        #region // rasterization

        #region // point

        /// <summary>
        /// Rasterize point.
        /// </summary>
        private void RasterizePoint(in PrimitivePoint primitive)
        {
            var x = (int)primitive.PositionScreen0.X;
            var y = (int)primitive.PositionScreen0.Y;

            StagePixelShader(x, y, default);
        }

        #endregion

        #region // line

        /// <summary>
        /// Rasterize line.
        /// </summary>
        private void RasterizeLine(in PrimitiveLine primitive)
        {
            var x0 = (int)primitive.PositionScreen0.X;
            var y0 = (int)primitive.PositionScreen0.Y;
            var x1 = (int)primitive.PositionScreen1.X;
            var y1 = (int)primitive.PositionScreen1.Y;

            // get pixel stream
            var pixels = BresenhamLine(x0, y0, x1, y1);

            // draw pixels
            foreach (var pixel in pixels)
            {
                StagePixelShader(pixel.X, pixel.Y, default);
            }
        }

        /// <summary>
        /// Bresenham's line algorithm line rasterization algorithm.
        /// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        /// </summary>
        public static IEnumerable<(int X, int Y)> BresenhamLine(int x0, int y0, int x1, int y1)
        {
            var w = x1 - x0;
            var h = y1 - y0;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            var longest = Math.Abs(w);
            var shortest = Math.Abs(h);
            if (longest <= shortest)
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            var numerator = longest >> 1;
            for (var i = 0; i <= longest; i++)
            {
                yield return (x0, y0);
                numerator += shortest;
                if (numerator < longest)
                {
                    x0 += dx2;
                    y0 += dy2;
                }
                else
                {
                    numerator -= longest;
                    x0 += dx1;
                    y0 += dy1;
                }
            }
        }

        #endregion

        #endregion
    }
}
