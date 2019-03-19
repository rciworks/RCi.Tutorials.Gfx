using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RCi.Tutorials.Gfx.Utils
{
    public static class U
    {
        /// <summary>
        /// Clamp value (ensure it falls into a given range).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(this int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
                return value;
            }
            if (value > max)
            {
                value = max;
            }
            return value;
        }

        /// <summary>
        /// <see cref="ICloneable.Clone"/> and cast it to explicit type <typeparam name="T"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Cloned<T>(this T cloneable) where T : ICloneable
        {
            return (T)cloneable.Clone();
        }

        /// <summary>
        /// Fill array with the same value.
        /// </summary>
        public static void Fill<T>(this T[] array, T value)
        {
            var length = array.Length;
            if (length == 0) return;

            // seed
            var seed = Math.Min(32, array.Length);
            for (var i = 0; i < seed; i++)
            {
                array[i] = value;
            }

            // copy by doubling
            int count;
            for (count = seed; count <= length / 2; count *= 2)
            {
                Array.Copy(array, 0, array, count, count);
            }

            // copy last part
            var leftover = length - count;
            if (leftover > 0)
            {
                Array.Copy(array, 0, array, count, leftover);
            }
        }

        /// <summary>
        /// Does <see cref="List{T}.ForEach"/> on <see cref="IEnumerable{T}"/> collection.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        /// <summary>
        /// Get handle of this window.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Handle(this System.Windows.Forms.Control window)
        {
            return window.IsDisposed ? default : Handle((System.Windows.Forms.IWin32Window)window);
        }

        /// <summary>
        /// Get handle of this window.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Handle(this System.Windows.Forms.IWin32Window window)
        {
            return window.Handle;
        }

        /// <summary>
        /// Get handle of this window.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Handle(this System.Windows.Media.Visual window)
        {
            var handleSource = window.HandleSource();
            return handleSource == null || handleSource.IsDisposed ? default : handleSource.Handle;
        }

        /// <summary>
        /// Object Lifetime:
        /// 
        /// An HwndSource is a regular common language runtime(CLR) object, and its lifetime is managed by the garbage collector.
        /// Because the HwndSource represents an unmanaged resource, HwndSource implements IDisposable.
        /// Synchronously calling Dispose immediately destroys the Win32 window if called from the owner thread.
        /// If called from another thread, the Win32 window is destroyed asynchronously.
        /// Calling Dispose explicitly from the interoperating code might be necessary for certain interoperation scenarios.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Windows.Interop.HwndSource HandleSource(this System.Windows.Media.Visual window)
        {
            return System.Windows.PresentationSource.FromVisual(window) as System.Windows.Interop.HwndSource;
        }

        /// <summary>
        /// Swap two instances.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref T value0, ref T value1)
        {
            var temp = value0;
            value0 = value1;
            value1 = temp;
        }

        /// <summary>
        /// Convert <see cref="System.Drawing.Color"/> to RGBA integer: 0xAABBGGRR.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToRgba(this System.Drawing.Color color)
        {
            return ((((color.A << 8) + color.B) << 8) + color.G << 8) + color.R;
        }

        /// <summary>
        /// Convert 0xAABBGGRR color <see cref="System.Drawing.Color"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Drawing.Color FromRgbaToColor(this int color)
        {
            return System.Drawing.Color.FromArgb
            (
                (color >> 24) & 0xFF,
                (color >> 0) & 0xFF,
                (color >> 8) & 0xFF,
                (color >> 16) & 0xFF
            );
        }
    }
}
