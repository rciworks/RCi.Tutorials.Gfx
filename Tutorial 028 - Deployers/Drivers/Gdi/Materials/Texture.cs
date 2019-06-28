using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <summary>
    /// GDI Texture.
    /// </summary>
    public class Texture :
        IDisposable
    {
        #region // storage

        /// <summary>
        /// Texture source.
        /// </summary>
        public DirectBitmap DirectBitmap { get; private set; }

        #endregion

        #region // ctor

        /// <summary />
        public Texture(Size size)
        {
            DirectBitmap = new DirectBitmap(size);
            DirectBitmap.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        /// <summary />
        public Texture(Image source) :
            this(source.Size)
        {
            DirectBitmap.Graphics.DrawImage
            (
                source,
                new Rectangle(Point.Empty, DirectBitmap.Size),
                new Rectangle(Point.Empty, source.Size),
                GraphicsUnit.Pixel
            );
        }

        /// <inheritdoc />
        public void Dispose()
        {
            DirectBitmap.Dispose();
            DirectBitmap = default;
        }

        #endregion
    }
}
