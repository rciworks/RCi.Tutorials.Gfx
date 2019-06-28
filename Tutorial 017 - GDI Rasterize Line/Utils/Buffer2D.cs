using System.Drawing;

namespace RCi.Tutorials.Gfx.Utils
{
    /// <summary>
    /// 2 dimensional buffer.
    /// </summary>
    public class Buffer2D<T>
    {
        #region // storage

        /// <summary>
        /// Size of the buffer.
        /// </summary>
        public Size Size { get; }

        /// <summary>
        /// Flattened array representing 2 dimensional buffer.
        /// </summary>
        public T[] Buffer { get; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public Buffer2D(Size size)
        {
            Size = size;
            Buffer = new T[Width * Height];
        }

        #endregion

        #region // routines

        /// <summary>
        /// Width of the buffer.
        /// </summary>
        public int Width => Size.Width;

        /// <summary>
        /// Height of the buffer.
        /// </summary>
        public int Height => Size.Height;

        /// <summary>
        /// Access data in 2 dimensional buffer.
        /// </summary>
        public T this[int x, int y]
        {
            get => GetValue(x, y);

            set => SetValue(x, y, value);
        }

        /// <summary>
        /// Get index from (x, y) in flattened <see cref="Buffer"/>.
        /// </summary>
        public int GetIndex(int x, int y) => x + y * Width;

        /// <summary>
        /// Get (x, y) from index in flattened <see cref="Buffer"/>.
        /// </summary>
        public (int x, int y) GetXY(int index)
        {
            var y = index / Width;
            var x = index - y * Width;
            return (x, y);
        }

        /// <summary>
        /// Set value at (x, y).
        /// </summary>
        public void SetValue(int x, int y, in T value) => Buffer[GetIndex(x, y)] = value;

        /// <summary>
        /// Get value at (x, y).
        /// </summary>
        public T GetValue(int x, int y) => Buffer[GetIndex(x, y)];

        /// <inheritdoc cref="U.Fill{T}"/>
        public void Clear(T value = default) => Buffer.Fill(value);

        #endregion
    }
}
