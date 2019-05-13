using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RCi.Tutorials.Gfx.Materials
{
    public static class TextureResourceLibrary
    {
        #region // storage

        /// <summary>
        /// Textures cache.
        /// </summary>
        private static Dictionary<int /* id = name hash */, ITextureResource> Cache { get; } = new Dictionary<int, ITextureResource>();

        #endregion

        #region // routines

        /// <summary>
        /// Get texture by id (directly from cache).
        /// </summary>
        public static ITextureResource Get(int id)
        {
            return Cache[id];
        }

        /// <summary>
        /// TryGet texture by id (directly from cache).
        /// </summary>
        public static bool TryGet(int id, out ITextureResource textureResource)
        {
            return Cache.TryGetValue(id, out textureResource);
        }

        /// <summary>
        /// TryGet texture by name (directly from cache).
        /// </summary>
        public static bool TryGet(string name, out ITextureResource textureResource)
        {
            return TryGet(name.GetHashCode(), out textureResource);
        }

        /// <summary>
        /// Get or create texture.
        /// </summary>
        private static ITextureResource GetOrCreate(string name, Func<TextureResource> ctorTextureResource)
        {
            var id = name.GetHashCode();
            if (!Cache.TryGetValue(id, out var textureResource))
            {
                Cache.Add(id, textureResource = ctorTextureResource());
            }
            return textureResource;
        }

        /// <summary>
        /// Get or create texture without source.
        /// </summary>
        public static ITextureResource GetOrCreate(string name, Size size)
        {
            return GetOrCreate(name, () => new TextureResource(name, size));
        }

        /// <summary>
        /// Get or create texture with source.
        /// </summary>
        public static ITextureResource GetOrCreate(string name, Func<Bitmap> getBitmap)
        {
            return GetOrCreate(name, () => new TextureResource(name, getBitmap));
        }

        /// <summary>
        /// Get or create texture from file.
        /// </summary>
        public static ITextureResource GetOrCreateFromFile(string filePathRelativeOrAbsolute)
        {
            var filePathAbsolute = new FileInfo(filePathRelativeOrAbsolute).FullName;
            return GetOrCreate(filePathAbsolute, () => new Bitmap(filePathAbsolute));
        }

        /// <summary>
        /// Delete texture.
        /// </summary>
        public static void Delete(ITextureResource textureResource)
        {
            Cache.Remove(textureResource.Id);
            textureResource.Dispose();
        }

        #endregion
    }
}
