using RCi.Tutorials.Gfx.Drivers.Gdi.Render;
using RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <inheritdoc />
    public abstract class Shader<TVsIn, TPsIn> :
        IShader<TVsIn, TPsIn>
        where TVsIn : unmanaged
        where TPsIn : unmanaged, IPsIn<TPsIn>
    {
        /// <inheritdoc />
        public RenderHost RenderHost { get; private set; }

        /// <inheritdoc />
        public IPipeline<TVsIn, TPsIn> Pipeline { get; private set; }

        #region // ctor

        /// <inheritdoc />
        protected Shader(RenderHost renderHost)
        {
            RenderHost = renderHost;
            Pipeline = new Pipeline<TVsIn, TPsIn>(this);
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            Pipeline.Dispose();
            Pipeline = default;

            RenderHost = default;
        }

        #endregion

        /// <inheritdoc />
        public abstract bool VertexShader(in TVsIn vsin, out TPsIn vsout);

        /// <inheritdoc />
        public abstract bool PixelShader(in TPsIn psin, out Vector4F color);
    }
}
