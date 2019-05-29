using System.Drawing;
using RCi.Tutorials.Gfx.Drivers.Gdi.Materials;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization
{
    /// <summary>
    /// Texture samples.
    /// </summary>
    public static class TextureSampler
    {
        /// <inheritdoc cref="Sample{T}"/>
        public static Color Sample(Texture texture, float u, float v)
        {
            return Color.FromArgb(Sample<int>(texture, u, v));
        }

        /// <summary>
        /// Sample pixel from texture.
        /// </summary>
        public static T Sample<T>(Texture texture, float u, float v)
            where T : unmanaged
        {
            var directBitmap = texture.DirectBitmap;
            // floor (instead of round) so it won't overflow 
            var x = (int)(u * directBitmap.Width);
            var y = (int)(v * directBitmap.Height);

            // wrap
            while (x < 0)
            {
                x += directBitmap.Width;
            }
            while (y < 0)
            {
                y += directBitmap.Height;
            }
            x %= directBitmap.Width;
            y %= directBitmap.Height;

            System.Diagnostics.Debug.Assert(x >= 0);
            System.Diagnostics.Debug.Assert(y >= 0);
            System.Diagnostics.Debug.Assert(x < directBitmap.Width);
            System.Diagnostics.Debug.Assert(y < directBitmap.Height);

            return directBitmap.Read<T>(x, y);
        }
    }
}
