using System;
using System.Collections.Generic;
using RCi.Tutorials.Gfx.Drivers.Gdi.Materials;
using RCi.Tutorials.Gfx.Materials;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render
{
    /// <inheritdoc />
    public class Pipeline<TVertexIn, TVertex> :
        IPipeline<TVertexIn, TVertex>
        where TVertexIn : struct
        where TVertex : struct, IVertex<TVertex/* TODO: temporary */>
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

        #region // routines

        /// <summary>
        /// Transform clip space to NDC to screen space.
        /// </summary>
        private void TransformClipToScreen(ref TVertex vertex)
        {
            // clip space to NDC to screen space
            var positionScreen = RenderHost.CameraInfo.Cache.MatrixViewport
                .Transform(vertex.Position)
                .ToVector3FNormalized()
                .ToVector4F(1);

            // clone and override
            vertex = vertex.CloneWithNewPosition(positionScreen);
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

            StagePrimitiveAssembly(verticesVsOut, primitiveTopology);
        }

        /// <summary>
        /// Primitive assembly stage.
        /// </summary>
        private void StagePrimitiveAssembly(TVertex[] vertices, PrimitiveTopology primitiveTopology)
        {
            switch (primitiveTopology)
            {
                case PrimitiveTopology.PointList:
                    for (var i = 0; i < vertices.Length; i++)
                    {
                        RasterizePoint(ref vertices[i]);
                    }
                    break;

                case PrimitiveTopology.LineList:
                    for (var i = 0; i < vertices.Length; i += 2)
                    {
                        RasterizeLine(ref vertices[i], ref vertices[i + 1]);
                    }
                    break;

                case PrimitiveTopology.LineStrip:
                    for (var i = 0; i < vertices.Length - 1; i++)
                    {
                        var copy0 = vertices[i];
                        var copy1 = vertices[i + 1];
                        RasterizeLine(ref copy0, ref copy1);
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
        private void StagePixelShader(int x, int y, ref TVertex vertex)
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

        #region // rasterize

        #region // point

        /// <summary>
        /// Rasterize point (input vertex in clip space).
        /// </summary>
        private void RasterizePoint(ref TVertex vertex0)
        {
            // TODO: clipping
            TransformClipToScreen(ref vertex0);
            DrawPoint(ref vertex0);
        }

        /// <summary>
        /// Draw point (input vertex in screen space).
        /// </summary>
        private void DrawPoint(ref TVertex vertex0)
        {
            var x = (int)vertex0.Position.X;
            var y = (int)vertex0.Position.Y;
            StagePixelShader(x, y, ref vertex0);
        }

        #endregion

        #region // line

        /// <summary>
        /// Rasterize line (input vertices in clip space).
        /// </summary>
        private void RasterizeLine(ref TVertex vertex0, ref TVertex vertex1)
        {
            // TODO: clipping
            TransformClipToScreen(ref vertex0);
            TransformClipToScreen(ref vertex1);
            DrawLine(ref vertex0, ref vertex1);
        }

        /// <summary>
        /// Draw line (input vertices in screen space).
        /// </summary>
        private void DrawLine(ref TVertex vertex0, ref TVertex vertex1)
        {
            // we're in screen space
            var x0 = (int)vertex0.Position.X;
            var y0 = (int)vertex0.Position.Y;
            var x1 = (int)vertex1.Position.X;
            var y1 = (int)vertex1.Position.Y;

            // TODO: vertex interpolation
            var empty = default(TVertex);

            // get pixel stream
            var pixels = BresenhamLine(x0, y0, x1, y1);

            // draw pixels
            foreach (var point in pixels)
            {
                StagePixelShader(point.X, point.Y, ref empty);
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
