using System.Linq;
using RCi.Tutorials.Gfx.Drivers.Gdi.Render;
using RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization;
using RCi.Tutorials.Gfx.Materials;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <inheritdoc cref="IGfxModel"/>
    public class GfxModel :
        IGfxModel
    {
        #region // storage

        /// <inheritdoc cref="RenderHost"/>
        public RenderHost RenderHost { get; set; }

        /// <inheritdoc cref="IModel"/>
        public IModel Model { get; set; }

        /// <inheritdoc cref="Texture"/>
        public Texture[] Textures { get; set; }

        #endregion

        #region // ctor

        /// <summary />
        public GfxModel(RenderHost renderHost, IModel model)
        {
            RenderHost = renderHost;
            Model = model;
            Textures = Model.TextureResources?.Select(tr => RenderHost.TextureLibrary.GetTexture(tr)).ToArray();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Textures = default;
            Model = default;
            RenderHost = default;
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        public void Render(IMaterial material, RenderContext renderContext)
        {
            material.Deploy<GfxModel, object>(this, renderContext);
        }

        public void Render<TVsIn, TPsIn>(IShader<TVsIn, TPsIn> shader, IBufferBinding<TVsIn> bufferBinding)
            where TVsIn : unmanaged
            where TPsIn : unmanaged, IPsIn<TPsIn>
        {
            shader.Pipeline.Render(bufferBinding, Model.Positions.Length, Model.PrimitiveTopology);
        }

        #endregion
    }
}
