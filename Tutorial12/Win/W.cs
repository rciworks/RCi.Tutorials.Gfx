using System;
using System.Text;

namespace RCi.Tutorials.Gfx.Win
{
    public static class W
    {
        #region // constants

        public const int WM_GETTEXTLENGTH = 0x000E;
        public const int WM_GETTEXT = 0x000D;

        #endregion

        /// <summary>
        /// Get client rectangle.
        /// </summary>
        public static System.Drawing.Rectangle GetClientRectangle(IntPtr handle)
        {
            User32.ClientToScreen(handle, out var point);
            User32.GetClientRect(handle, out var rect);
            return new System.Drawing.Rectangle(point.X, point.Y, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        /// <summary>
        /// Get window title/caption.
        /// </summary>
        public static string GetWindowCaption(IntPtr hwnd)
        {
            var lpWindowText = new StringBuilder(byte.MaxValue);
            User32.GetWindowText(hwnd, lpWindowText, lpWindowText.Capacity + 1);
            return lpWindowText.ToString();
        }

        /// <summary>
        /// Get window class name.
        /// </summary>
        public static string GetWindowClassName(IntPtr hwnd)
        {
            var sb = new StringBuilder(byte.MaxValue);
            return User32.GetClassName(hwnd, sb, sb.Capacity) != 0 ? sb.ToString() : null;
        }

        /// <summary>
        /// Get window rectangle.
        /// </summary>
        public static System.Drawing.Rectangle GetWindowRectangle(IntPtr hwnd)
        {
            User32.GetWindowRect(hwnd, out var rect);
            return new System.Drawing.Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        /// <summary>
        /// Get window raw text.
        /// </summary>
        public static string GetWindowTextRaw(IntPtr hwnd)
        {
            // get correct string length
            var length = User32.SendMessage(hwnd, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero).ToInt32();
            var sb = new StringBuilder(length + 1);

            // get actual string
            User32.SendMessage(hwnd, WM_GETTEXT, new IntPtr(sb.Capacity), sb);
            return sb.ToString();
        }
    }
}
