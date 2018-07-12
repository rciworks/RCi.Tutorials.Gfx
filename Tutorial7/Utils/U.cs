using System;
using System.Collections.Generic;

namespace RCi.Tutorials.Gfx.Utils
{
    public static class U
    {
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
        public static IntPtr Handle(this System.Windows.Forms.Control window)
        {
            return window.IsDisposed ? default : Handle((System.Windows.Forms.IWin32Window)window);
        }

        /// <summary>
        /// Get handle of this window.
        /// </summary>
        public static IntPtr Handle(this System.Windows.Forms.IWin32Window window)
        {
            return window.Handle;
        }

        /// <summary>
        /// Get handle of this window.
        /// </summary>
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
        public static System.Windows.Interop.HwndSource HandleSource(this System.Windows.Media.Visual window)
        {
            return System.Windows.PresentationSource.FromVisual(window) as System.Windows.Interop.HwndSource;
        }
    }
}
