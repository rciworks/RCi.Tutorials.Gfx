using System;
using System.Collections.Generic;
using RCi.Tutorials.Gfx.Drivers.Gdi.Render;
using RCi.Tutorials.Gfx.Materials;

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

        /// <summary>
        /// <see cref="ShaderType.Position"/> shader.
        /// </summary>
        public Position.Shader ShaderPosition { get; }

        /// <summary>
        /// <see cref="ShaderType.PositionColor"/> shader.
        /// </summary>
        public PositionColor.Shader ShaderPositionColor { get; }

        /// <summary>
        /// <see cref="ShaderType.PositionTexture"/> shader.
        /// </summary>
        public PositionTexture.Shader ShaderPositionTexture { get; }

        #endregion

        #region // ctor

        /// <summary />
        public ShaderLibrary(RenderHost renderHost)
        {
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
