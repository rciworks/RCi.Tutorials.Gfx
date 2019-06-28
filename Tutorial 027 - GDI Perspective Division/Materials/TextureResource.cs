using System;
using System.Drawing;
using System.Drawing.Imaging;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <inheritdoc cref="ITextureResource"/>
    public class TextureResource :
        ITextureResource
    {
        #region // storage

        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public Size Size { get; private set; }

        /// <inheritdoc />
        public Bitmap Source { get; private set; }

        #endregion

        #region // ctor

        /// <summary />
        internal TextureResource(string name, Size size)
        {
            Name = name;
            Id = Name.GetHashCode();
            Size = size;
        }

        /// <summary />
        internal TextureResource(string name, Func<Bitmap> getSource)
        {
            Name = name;
            Id = Name.GetHashCode();

            // get initial data
            var source = getSource();
            // ensure it's 32bit argb
            if (source.PixelFormat != PixelFormat.Format32bppArgb)
            {
                var bitmap32argb = source.ToBitmap(Color.FromArgb);
                source.Dispose();
                source = bitmap32argb;
            }

            Source = source;
            Size = Source.Size;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Source?.Dispose();
            Source = default;

            Size = default;
            Name = default;
        }

        #endregion

        #region // hashing

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id;
        }

        /// <inheritdoc />
        public int GetHashCode(ITextureResource obj)
        {
            return obj.GetHashCode();
        }

        /// <summary />
        public static bool AreEqual(ITextureResource left, ITextureResource right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.GetHashCode() == right.GetHashCode();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is ITextureResource textureResource)
            {
                return AreEqual(this, textureResource);
            }
            return false;
        }

        /// <inheritdoc />
        public bool Equals(ITextureResource left, ITextureResource right)
        {
            return AreEqual(left, right);
        }

        #endregion
    }
}
