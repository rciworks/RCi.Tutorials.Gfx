using System;
using RCi.Tutorials.Gfx.Drivers.Gdi.Materials;
using RCi.Tutorials.Gfx.Materials;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization
{
    /// <summary>
    /// Graphics pipeline interface.
    /// </summary>
    public interface IPipeline<in TVsIn, TPsIn> :
        IDisposable
        where TVsIn : unmanaged
        where TPsIn : unmanaged, IPsIn<TPsIn>
    {
        /// <summary>
        /// Render vertices.
        /// </summary>
        void Render(IBufferBinding<TVsIn> bufferBinding, int countVertices, PrimitiveTopology primitiveTopology);
    }
}
