using System;
using RCi.Tutorials.Gfx.Drivers.Gdi.Render;
using RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization;
using RCi.Tutorials.Gfx.Materials;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <inheritdoc cref="IGfxModel"/>
    public abstract class GfxModel :
        IGfxModel
    {
        #region // storage

        /// <inheritdoc cref="RenderHost"/>
        protected RenderHost RenderHost { get; set; }

        /// <inheritdoc cref="IModel"/>
        public IModel Model { get; set; }

        /// <inheritdoc cref="Texture"/>
        public Texture Texture { get; set; }

        #endregion

        #region // ctor

        /// <summary />
        protected GfxModel(RenderHost renderHost, IModel model)
        {
            RenderHost = renderHost;
            Model = model;
            if (!(Model.TextureResource is null))
            {
                Texture = RenderHost.TextureLibrary.GetTexture(Model.TextureResource);
            }
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            Texture = default;
            Model = default;
            RenderHost = default;
        }

        #endregion

        #region // routines

        /// <summary>
        /// Construct graphics model.
        /// </summary>
        public static IGfxModel Factory(RenderHost renderHost, IModel model)
        {
            // TODO: solve without switch
            switch (model.ShaderType)
            {
                case ShaderType.Position:
                    return new Position.GfxModel(renderHost, model);

                case ShaderType.PositionColor:
                    return new PositionColor.GfxModel(renderHost, model);

                case ShaderType.PositionTexture:
                    throw new NotImplementedException(); // TODO:

                default:
                    throw new ArgumentOutOfRangeException(nameof(model.ShaderType), model.ShaderType, default);
            }
        }

        /// <summary>
        /// Update shader.
        /// </summary>
        protected abstract void ShaderUpdate(in Matrix4D matrixToClip);

        /// <summary>
        /// Render.
        /// </summary>
        protected abstract void Render();

        /// <inheritdoc />
        public void Render(in Matrix4D matrixToClip)
        {
            ShaderUpdate(matrixToClip);
            Render();
        }

        #endregion
    }

    /// <inheritdoc cref="GfxModel"/>
    public abstract class GfxModel<TVsIn, TPsIn> :
        GfxModel
        where TVsIn : unmanaged
        where TPsIn : unmanaged, IPsIn<TPsIn>
    {
        #region // storage

        /// <inheritdoc cref="IShader{TVsIn,TPsIn}"/>
        private IShader<TVsIn, TPsIn> Shader { get; set; }

        /// <inheritdoc cref="IBufferBinding{TVsIn}"/>
        private IBufferBinding<TVsIn> BufferBinding { get; set; }

        #endregion

        #region // ctor

        /// <summary />
        protected GfxModel(RenderHost renderHost, IModel model, IShader<TVsIn, TPsIn> shader, IBufferBinding<TVsIn> bufferBinding) :
            base(renderHost, model)
        {
            Shader = shader;
            BufferBinding = bufferBinding;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            Shader = default;
            BufferBinding = default;

            base.Dispose();
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        protected override void Render()
        {
            Shader.Pipeline.Render(BufferBinding, Model.Positions.Length, Model.PrimitiveTopology);
        }

        #endregion
    }
}
