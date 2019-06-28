using System;
using System.Collections.Generic;
using RCi.Tutorials.Gfx.Drivers.Gdi.Render;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <summary>
    /// Shader library.
    /// </summary>
    public class ShaderLibrary :
        IDisposable
    {
        #region // storage

        /// <summary>
        /// All created shaders.
        /// </summary>
        private List<IShader> Shaders { get; set; } = new List<IShader>();

        /// <inheritdoc cref="Default.Shader"/>
        public Default.Shader ShaderDefault { get; }

        /// <inheritdoc cref="Position.Shader"/>
        public Position.Shader ShaderPosition { get; }

        /// <inheritdoc cref="PositionColor.Shader"/>
        public PositionColor.Shader ShaderPositionColor { get; }

        /// <inheritdoc cref="PositionTexture.Shader"/>
        public PositionTexture.Shader ShaderPositionTexture { get; }

        #endregion

        #region // ctor

        /// <summary />
        public ShaderLibrary(RenderHost renderHost)
        {
            Shaders.Add(ShaderDefault = new Default.Shader(renderHost));
            Shaders.Add(ShaderPosition = new Position.Shader(renderHost));
            Shaders.Add(ShaderPositionColor = new PositionColor.Shader(renderHost));
            Shaders.Add(ShaderPositionTexture = new PositionTexture.Shader(renderHost));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Shaders.ForEach(shader => shader.Dispose());
            Shaders = default;
        }

        #endregion
    }
}
