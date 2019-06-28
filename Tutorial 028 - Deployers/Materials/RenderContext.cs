using RCi.Tutorials.Gfx.Common.Camera;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Generic rendering context independent from visuals.
    /// </summary>
    public class RenderContext
    {
        /// <summary>
        /// Matrix from any arbitrary <see cref="Space"/> to clipping space.
        /// </summary>
        public Matrix4D MatrixToClip { get; }

        /// <inheritdoc cref="Viewport"/>
        public Viewport Viewport { get; }

        /// <summary />
        public RenderContext(in Matrix4D matrixToClip, in Viewport viewport)
        {
            MatrixToClip = matrixToClip;
            Viewport = viewport;
        }
    }
}
