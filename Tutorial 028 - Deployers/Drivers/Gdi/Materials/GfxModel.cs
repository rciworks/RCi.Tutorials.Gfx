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
        protected RenderHost RenderHost { get; set; }

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
            // TODO: generic solution!
            switch (material)
            {
                case Gfx.Materials.Default.Material _:
                    var shaderDefault = RenderHost.ShaderLibrary.ShaderDefault;
                    shaderDefault.Update(renderContext.MatrixToClip);
                    Render(shaderDefault, new Default.BufferBinding(Model.Positions));
                    break;

                case Gfx.Materials.Position.Material materialPosition:
                    var shaderPosition = RenderHost.ShaderLibrary.ShaderPosition;
                    shaderPosition.Update(renderContext.MatrixToClip, materialPosition.Color);
                    Render(shaderPosition, new Position.BufferBinding(Model.Positions));
                    break;

                case Gfx.Materials.PositionColor.Material _:
                    var shaderPositionColor = RenderHost.ShaderLibrary.ShaderPositionColor;
                    shaderPositionColor.Update(renderContext.MatrixToClip);
                    Render(shaderPositionColor, new PositionColor.BufferBinding(Model.Positions, Model.Colors));
                    break;

                case Gfx.Materials.PositionTexture.Material _:
                    var shaderPositionTexture = RenderHost.ShaderLibrary.ShaderPositionTexture;
                    shaderPositionTexture.Update(renderContext.MatrixToClip, Textures[0]);
                    Render(shaderPositionTexture, new PositionTexture.BufferBinding(Model.Positions, Model.TextureCoordinates));
                    break;
            }
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
