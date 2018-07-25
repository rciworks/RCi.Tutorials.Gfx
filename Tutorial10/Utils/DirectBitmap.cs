using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RCi.Tutorials.Gfx.Utils
{
    /// <summary>
    /// Bitmap wrapper for direct access to its memory.
    /// </summary>
    public class DirectBitmap :
        IDisposable
    {
        #region // storage

        /// <summary>
        /// Size of <see cref="Bitmap"/>.
        /// </summary>
        public Size Size { get; }

        /// <summary>
        /// Width of <see cref="Bitmap"/>.
        /// </summary>
        public int Width => Size.Width;

        /// <summary>
        /// Height of <see cref="Bitmap"/>.
        /// </summary>
        public int Height => Size.Height;

        /// <summary>
        /// Actual buffer which is used for <see cref="Bitmap"/>.
        /// </summary>
        public int[] Buffer { get; private set; }

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
        public DirectBitmap(Size size)
        {
            Size = size;
            Buffer = new int[Width * Height];
            BufferHandle = GCHandle.Alloc(Buffer, GCHandleType.Pinned);
            Bitmap = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppPArgb, BufferHandle.AddrOfPinnedObject());
            Graphics = Graphics.FromImage(Bitmap);
        }

        /// <inheritdoc />
        public DirectBitmap(int width, int height) :
            this(new Size(width, height))
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Graphics.Dispose();
            Graphics = default;

            Bitmap.Dispose();
            Bitmap = default;

            BufferHandle.Free();
            BufferHandle = default;

            Buffer = default;
        }

        #endregion

        #region // routines

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetIndex(int x, int y) => x + y * Width;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetXY(int index, out int x, out int y)
        {
            y = index / Width;
            x = index - y * Width;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetArgb(int x, int y, int argb) => Buffer[GetIndex(x, y)] = argb;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetArgb(int x, int y) => Buffer[GetIndex(x, y)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPixel(int x, int y, Color color) => SetArgb(x, y, color.ToArgb());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color GetPixel(int x, int y) => Color.FromArgb(GetArgb(x, y));

        #endregion
    }
}
