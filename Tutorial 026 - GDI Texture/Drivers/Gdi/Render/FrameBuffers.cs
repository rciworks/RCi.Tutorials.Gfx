using System;
using System.Drawing;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render
{
    /// <summary>
    /// Collection of various buffers for one view.
    /// </summary>
    public class FrameBuffers :
        IDisposable
    {
        #region // storage

        /// <summary>
        /// Count of color buffers.
        /// </summary>
        public const int LayerCount = 1;

        /// <summary>
        /// Size of all buffers.
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// Collection of color buffers.
        /// </summary>
        public DirectBitmap[] BufferColor { get; set; }

        /// <summary>
        /// Depth buffer.
        /// </summary>
        public Buffer2D<float> BufferDepth { get; set; }

        #endregion

        #region // ctor

        /// <summary />
        public FrameBuffers(Size size)
        {
            Size = size;

            BufferColor = new DirectBitmap[LayerCount];
            for (var i = 0; i < BufferColor.Length; i++)
            {
                BufferColor[i] = new DirectBitmap(Size);
            }

            BufferDepth = new Buffer2D<float>(Size);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            BufferColor.ForEach(o => o.Dispose());
            BufferColor = default;

            BufferDepth.Dispose();
            BufferDepth = default;

            Size = default;
        }

        #endregion
    }
}
