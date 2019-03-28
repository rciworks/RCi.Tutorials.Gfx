using RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.PositionColor
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
        /// Color buffer.
        /// </summary>
        public int[] Colors { get; }

        /// <summary />
        public BufferBinding(Vector3F[] positions, int[] colors)
        {
            Positions = positions;
            Colors = colors;
        }

        /// <inheritdoc />
        public VsIn GetVsIn(uint index)
        {
            return new VsIn
            (
                Positions[index],
                Colors[index].FromRgbaToVector4F()
            );
        }
    }
}
