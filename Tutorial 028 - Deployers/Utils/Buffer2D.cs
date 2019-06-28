using System;
using System.Drawing;

namespace RCi.Tutorials.Gfx.Utils
{
    /// <summary>
    /// 2 dimensional buffer.
    /// </summary>
    public class Buffer2D<T> :
        Buffer<T>
        where T : unmanaged
    {
        #region // storage

        /// <summary>
        /// Size of the buffer.
        /// </summary>
        public Size Size { get; }

        #endregion

        #region // ctor

        public Buffer2D(Size size, T[] data) :
            base(data)
        {
            if (size.Width * size.Height != data.Length)
            {
                throw new ArgumentException("Invalid data.");
            }
            Size = size;
        }

        public Buffer2D(Size size) :
            this(size, new T[size.Width * size.Height])
        {
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
            get => Read<T>(x, y);
            set => Write(x, y, value);
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
        public void Write<U>(int x, int y, U value)
            where U : unmanaged
        {
            Write(GetIndex(x, y), value);
        }

        /// <summary>
        /// Get value at (x, y).
        /// </summary>
        public U Read<U>(int x, int y)
            where U : unmanaged
        {
            return Read<U>(GetIndex(x, y));
        }

        /// <summary>
        /// Get value at (x, y).
        /// </summary>
        public T Read(int x, int y)
        {
            return Read<T>(x, y);
        }

        /// <inheritdoc cref="U.Fill{T}"/>
        public void Clear(T value = default) => Data.Fill(value);

        #endregion
    }
}
