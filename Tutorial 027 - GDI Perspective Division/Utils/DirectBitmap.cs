using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace RCi.Tutorials.Gfx.Utils
{
    /// <summary>
    /// Bitmap wrapper for direct access to its memory.
    /// </summary>
    public class DirectBitmap :
        Buffer2D<int>
    {
        #region // storage

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

        public DirectBitmap(Size size, int[] data) :
            base(size, data)
        {
            Bitmap = new Bitmap(Width, Height, Width * sizeof(int), PixelFormat.Format32bppPArgb, Address);
            Graphics = Graphics.FromImage(Bitmap);
        }

        /// <inheritdoc />
        public DirectBitmap(Size size) :
            this(size, new int[size.Width * size.Height])
        {
        }

        /// <inheritdoc />
        public DirectBitmap(int width, int height) :
            this(new Size(width, height))
        {
        }

        /// <inheritdoc cref="IDisposable" />
        public override void Dispose()
        {
            Graphics.Dispose();
            Graphics = default;

            Bitmap.Dispose();
            Bitmap = default;

            base.Dispose();
        }

        #endregion

        #region // routines

        /// <summary>
        /// Set pixel color at (x, y).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPixel(int x, int y, Color color) => Write(x, y, color.ToArgb());

        /// <summary>
        /// Get pixel color at (x, y).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color GetPixel(int x, int y) => Color.FromArgb(Read<int>(x, y));

        /// <summary>
        /// Clear buffer by one color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(Color color) => Clear(color.ToArgb());

        #endregion
    }
}
