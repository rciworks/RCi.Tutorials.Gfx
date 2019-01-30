using System;
using RCi.Tutorials.Gfx.Drivers.Gdi.Render;
using RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <summary>
    /// Shader interface.
    /// </summary>
    public interface IShader :
        IDisposable
    {
        /// <summary>
        /// Render host which owns this shader.
        /// </summary>
        RenderHost RenderHost { get; }
    }

    public interface IShader<TVsIn, TPsIn> :
        IShader
        where TVsIn : unmanaged
        where TPsIn : unmanaged, IPsIn<TPsIn>
    {
        /// <summary>
        /// Mounted pipeline.
        /// </summary>
        IPipeline<TVsIn, TPsIn> Pipeline { get; }

        /// <summary>
        /// Execute vertex shader.
        /// </summary>
        bool VertexShader(in TVsIn vsin, out TPsIn vsout);

        /// <summary>
        /// Execute pixel (fragment) shader.
        /// </summary>
        bool PixelShader(in TPsIn psin, out Vector4F color);
    }
}
