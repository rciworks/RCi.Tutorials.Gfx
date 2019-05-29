using RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.PositionTexture
{
    /// <inheritdoc cref="IBufferBinding{VsIn}" />
    public class BufferBinding
        : IBufferBinding<VsIn>
    {
        /// <summary>
        /// Position buffer.
        /// </summary>
        public Vector3F[] Positions { get; }

        /// <summary>
        /// Texture coordinate buffer.
        /// </summary>
        public Vector2F[] TextureCoordinates { get; }

        /// <summary />
        public BufferBinding(Vector3F[] positions, Vector2F[] textureCoordinates)
        {
            Positions = positions;
            TextureCoordinates = textureCoordinates;
        }

        /// <inheritdoc />
        public VsIn GetVsIn(uint index)
        {
            return new VsIn
            (
                Positions[index],
                TextureCoordinates[index]
            );
        }
    }
}
