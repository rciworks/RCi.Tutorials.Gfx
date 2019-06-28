using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RCi.Tutorials.Gfx.Utils
{
    /// <summary>
    /// Bitmap wrapper for direct access to its memory.
    /// </summary>
    public class DirectBitmap :
        Buffer2D<int>,
        IDisposable
    {
        #region // storage

        /// <summary>
        /// Pinned GC handle to <see cref="Buffer"/>.
        /// </summary>
        private GCHandle BufferHandle { get; set; }

        /// <summary>
        /// Bitmap constructed from <see cref="Buffer"/>.
        /// </summary>
        public Bitmap Bitmap { get; private set; }

        /// <summary>
        /// <see cref="Graphics"/> of <see cref="Bitmap"/>.
        /// </summary>
        public Graphics Graphics { get; private set; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public DirectBitmap(Size size) :
            base(size)
        {
            BufferHandle = GCHandle.Alloc(Buffer, GCHandleType.Pinned);
            Bitmap = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppPArgb, BufferHandle.AddrOfPinnedObject());
            Graphics = Graphics.FromImage(Bitmap);
        }

        /// <inheritdoc />
        public DirectBitmap(int width, int height) :
            this(new Size(width, height))
        {
        }

        /// <inheritdoc cref="IDisposable" />
        public void Dispose()
        {
            Graphics.Dispose();
            Graphics = default;

            Bitmap.Dispose();
            Bitmap = default;

            BufferHandle.Free();
            BufferHandle = default;
        }

        #endregion

        #region // routines

        /// <summary>
        /// Set pixel color at (x, y).
        /// </summary>
        public void SetPixel(int x, int y, Color color) => SetValue(x, y, color.ToArgb());

        /// <summary>
        /// Get pixel color at (x, y).
        /// </summary>
        public Color GetPixel(int x, int y) => Color.FromArgb(GetValue(x, y));

        /// <summary>
        /// Clear buffer by one color.
        /// </summary>
        public void Clear(Color color) => Clear(color.ToArgb());

        #endregion
    }
}
