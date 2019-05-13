using System;
using System.Collections.Generic;
using System.Linq;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Texture library of known <typeparamref name="TTextureResource"/> and <typeparamref name="TTexture"/>.
    /// </summary>
    public abstract class TextureLibrary<TTextureResource, TTexture> :
        IDisposable
        where TTextureResource : ITextureResource
    {
        #region // storage

        /// <summary>
        /// Texture cache.
        /// </summary>
        protected Dictionary<TTextureResource, TTexture> Textures { get; set; } = new Dictionary<TTextureResource, TTexture>();

        #endregion

        #region // ctor

        /// <inheritdoc />
        public virtual void Dispose()
        {
            Textures.Select(pair => pair.Key).ToArray().ForEach(DeleteTexture);
            Textures = default;
        }

        #endregion

        #region // routines

        /// <summary>
        /// Get (or create) <typeparamref name="TTexture"/>.
        /// </summary>
        public TTexture GetTexture(TTextureResource textureResource)
        {
            if (!Textures.TryGetValue(textureResource, out var texture))
            {
                Textures.Add(textureResource, texture = CreateTexture(textureResource));
            }
            return texture;
        }

        /// <summary>
        /// Create <typeparamref name="TTexture"/> based on <typeparamref name="TTextureResource"/>.
        /// </summary>
        protected abstract TTexture CreateTexture(TTextureResource textureResource);

        /// <summary>
        /// Delete <typeparamref name="TTexture"/> knowing <typeparamref name="TTextureResource"/>.
        /// </summary>
        public void DeleteTexture(TTextureResource textureResource)
        {
            var texture = Textures[textureResource];
            DeleteTexture(texture);
            Textures.Remove(textureResource);
        }

        /// <summary>
        /// Delete <typeparamref name="TTexture"/>.
        /// </summary>
        protected abstract void DeleteTexture(TTexture texture);

        #endregion
    }
}
