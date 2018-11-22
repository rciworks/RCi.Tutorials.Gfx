using System;
using RCi.Tutorials.Gfx.Drivers.Gdi.Materials;
using RCi.Tutorials.Gfx.Materials;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render
{
    /// <inheritdoc />
    public class Pipeline<TVertex, TVertexShader> :
        IPipeline<TVertex, TVertexShader>
        where TVertex : struct
        where TVertexShader : struct, IVertexShader<TVertexShader/* TODO: temporary */>
    {
        #region // singleton

        /// <summary>
        /// Singleton access.
        /// </summary>
        public static IPipeline<TVertex, TVertexShader> Instance { get; } = new Pipeline<TVertex, TVertexShader>();

        #endregion

        #region // storage

        /// <summary>
        /// Ongoing render host.
        /// </summary>
        private RenderHost RenderHost { get; set; }

        /// <summary>
        /// Ongoing shader.
        /// </summary>
        private IShader<TVertex, TVertexShader> Shader { get; set; }

        #endregion

        #region // public access

        /// <inheritdoc />
        public void SetRenderHost(RenderHost renderHost) => RenderHost = renderHost;

        /// <inheritdoc />
        public void SetShader(IShader<TVertex, TVertexShader> shader) => Shader = shader;

        /// <inheritdoc />
        public void Render(TVertex[] vertices, PrimitiveTopology primitiveTopology)
        {
            StageVertexShader(vertices, primitiveTopology);
        }

        #endregion

        #region // routines

        /// <summary>
        /// Transform clip space to NDC to screen space.
        /// </summary>
        private void TransformClipToScreen(ref TVertexShader vertex)
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
        private void StageVertexShader(TVertex[] vertices, PrimitiveTopology primitiveTopology)
        {
            var verticesVsOut = new TVertexShader[vertices.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                verticesVsOut[i] = Shader.VertexShader(vertices[i]);
            }

            StagePrimitiveAssembly(verticesVsOut, primitiveTopology);
        }

        /// <summary>
        /// Primitive assembly stage.
        /// </summary>
        private void StagePrimitiveAssembly(TVertexShader[] vertices, PrimitiveTopology primitiveTopology)
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
                    // TODO:
                    break;

                case PrimitiveTopology.LineStrip:
                    // TODO:
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
        private void StagePixelShader(int x, int y, ref TVertexShader vertex)
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
        /// Rasterize point (in clipping space).
        /// </summary>
        private void RasterizePoint(ref TVertexShader vertex0)
        {
            TransformClipToScreen(ref vertex0);
            DrawPoint(ref vertex0);
        }

        /// <summary>
        /// Draw point (in screen space).
        /// </summary>
        private void DrawPoint(ref TVertexShader vertex0)
        {
            var x = (int)vertex0.Position.X;
            var y = (int)vertex0.Position.Y;
            StagePixelShader(x, y, ref vertex0);
        }

        #endregion

        #endregion
    }
}
