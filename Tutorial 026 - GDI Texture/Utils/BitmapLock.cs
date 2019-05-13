using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RCi.Tutorials.Gfx.Utils
{
    public class BitmapLock
    {
        #region // storage

        private Bitmap m_Source { get; }
        private IntPtr m_SourceHandle { get; set; }
        private BitmapData m_BitmapData { get; set; }
        private int m_BytesPerPixel { get; }
        private byte[] m_Pixels { get; set; }
        private Func<int, int, Color> m_GetPixel { get; }
        private Action<int, int, Color> m_SetPixel { get; }

        public int Width { get; }
        public int Height { get; }
        public PixelFormat PixelFormat { get; }
        public Color GetPixel(int x, int y) => m_GetPixel(x, y);
        public void SetPixel(int x, int y, Color color) => m_SetPixel(x, y, color);
        public BitmapData BitmapData => m_BitmapData;

        #endregion

        #region // ctor

        public BitmapLock(Bitmap source)
        {
            m_Source = source;
            Width = m_Source.Width;
            Height = m_Source.Height;
            PixelFormat = m_Source.PixelFormat;

            switch (PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    m_BytesPerPixel = 3;
                    m_GetPixel = GetPixel_Format24bppRgb;
                    m_SetPixel = SetPixel_Format24bppRgb;
                    break;

                case PixelFormat.Format32bppArgb:
                    m_BytesPerPixel = 4;
                    m_GetPixel = GetPixel_Format32bppArgb;
                    m_SetPixel = SetPixel_Format32bppArgb;
                    break;

                default:
                    throw new NotSupportedException($"'{nameof(BitmapLock)}' for pixel format '{PixelFormat}' is not supported.");
            }
        }

        #endregion

        #region // lock / unlock

        public void LockBits(ImageLockMode imageLockMode)
        {
            // Create rectangle to lock
            var rect = new Rectangle(0, 0, Width, Height);

            // Lock bitmap and return bitmap data
            m_BitmapData = m_Source.LockBits(rect, imageLockMode, m_Source.PixelFormat);

            // create byte array to copy pixel values
            m_Pixels = new byte[Width * Height * m_BytesPerPixel];
            m_SourceHandle = m_BitmapData.Scan0;

            // Copy data from pointer to array
            Marshal.Copy(m_SourceHandle, m_Pixels, 0, m_Pixels.Length);
        }

        public void UnlockBits()
        {
            // Copy data from byte array to pointer
            Marshal.Copy(m_Pixels, 0, m_SourceHandle, m_Pixels.Length);

            // Unlock bitmap data
            m_Source.UnlockBits(m_BitmapData);
        }

        #endregion

        #region // Format24bppRgb

        private Color GetPixel_Format24bppRgb(int x, int y)
        {
            var offset = (y * Width + x) * 3;
            var b = m_Pixels[offset++];
            var g = m_Pixels[offset++];
            var r = m_Pixels[offset];
            return Color.FromArgb(255, r, g, b);
        }

        private void SetPixel_Format24bppRgb(int x, int y, Color color)
        {
            var offset = (y * Width + x) * 3;
            m_Pixels[offset++] = color.B;
            m_Pixels[offset++] = color.G;
            m_Pixels[offset] = color.R;
        }

        #endregion

        #region // Format32bppArgb

        private Color GetPixel_Format32bppArgb(int x, int y)
        {
            var offset = (y * Width + x) * 4;
            var b = m_Pixels[offset++];
            var g = m_Pixels[offset++];
            var r = m_Pixels[offset++];
            var a = m_Pixels[offset];
            return Color.FromArgb(a, r, g, b);
        }

        private void SetPixel_Format32bppArgb(int x, int y, Color color)
        {
            var offset = (y * Width + x) * 4;
            m_Pixels[offset++] = color.B;
            m_Pixels[offset++] = color.G;
            m_Pixels[offset++] = color.R;
            m_Pixels[offset] = color.A;
        }

        #endregion
    }

    public static class BitmapLockEx
    {
        public static Bitmap ToBitmap(this Bitmap bitmap, Func<int /* A[0-255] */, int /* R[0-255] */, int /* G[0-255] */, int /* B[0-255] */, Color> getColorFromArgb)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;

            var bitmapLock = new BitmapLock(bitmap);
            var bitmap32 = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var bitmap32Lock = new BitmapLock(bitmap32);

            bitmapLock.LockBits(ImageLockMode.ReadOnly);
            bitmap32Lock.LockBits(ImageLockMode.WriteOnly);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var pixel = bitmapLock.GetPixel(x, y);
                    pixel = getColorFromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
                    bitmap32Lock.SetPixel(x, y, pixel);
                }
            }

            bitmap32Lock.UnlockBits();
            bitmapLock.UnlockBits();

            return bitmap32;
        }
    }
}
