using RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.Position
{
    /// <inheritdoc />
    public class BufferBinding
        : IBufferBinding<VsIn>
    {
        /// <summary>
        /// Position buffer.
        /// </summary>
        public Vector3F[] Positions { get; }

        /// <summary />
        public BufferBinding(Vector3F[] positions)
        {
            Positions = positions;
        }

        /// <inheritdoc />
        public VsIn GetVsIn(uint index)
        {
            return new VsIn
            (
                Positions[index]
            );
        }
    }
}
