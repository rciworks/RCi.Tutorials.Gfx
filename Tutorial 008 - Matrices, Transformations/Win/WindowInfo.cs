using System;
using System.Collections.Generic;

namespace RCi.Tutorials.Gfx.Win
{
    public class WindowInfo
    {
        #region // storage

        /// <summary>
        /// Handle of targeted window.
        /// </summary>
        public IntPtr Handle { get; }

        /// <summary>
        /// Parent handle of this window.
        /// </summary>
        public IntPtr ParentHandle { get; }

        /// <summary>
        /// Class name.
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// Title/caption of window.
        /// </summary>
        public string Caption { get; }

        /// <summary>
        /// Raw text of window.
        /// </summary>
        public string RawText { get; }

        /// <summary>
        /// Window rectangle.
        /// </summary>
        public System.Drawing.Rectangle RectangleWindow { get; }

        /// <summary>
        /// Client rectangle (local).
        /// </summary>
        public System.Drawing.Rectangle RectangleClientLocal { get; }

        /// <summary>
        /// Client rectangle (global).
        /// </summary>
        public System.Drawing.Rectangle RectangleClientAbsolute { get; }

        /// <summary>
        /// Is window visible?
        /// </summary>
        public bool IsWindowVisible { get; }

        /// <summary>
        /// Parent <see cref="WindowInfo"/> (if provided).
        /// </summary>
        public WindowInfo Parent { get; }

        /// <summary>
        /// Children of window (if requested).
        /// </summary>
        public List<WindowInfo> Children { get; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public WindowInfo(IntPtr handle, WindowInfo parent, bool getChildren)
        {
            // store
            Children = new List<WindowInfo>();
            Handle = handle;
            Parent = parent;

            // get properties
            ParentHandle = User32.GetParent(Handle);
            ClassName = W.GetWindowClassName(Handle);
            Caption = W.GetWindowCaption(Handle);
            RawText = W.GetWindowTextRaw(Handle);
            RectangleWindow = W.GetWindowRectangle(Handle);
            RectangleClientAbsolute = W.GetClientRectangle(Handle);
            RectangleClientLocal = new System.Drawing.Rectangle
            (
                RectangleClientAbsolute.X - RectangleWindow.X,
                RectangleClientAbsolute.Y - RectangleWindow.Y,
                RectangleClientAbsolute.Width,
                RectangleClientAbsolute.Height
            );
            IsWindowVisible = User32.IsWindowVisible(Handle);

            // collect children
            if (getChildren)
            {
                CollectChildren();
            }
        }

        /// <inheritdoc />
        public WindowInfo(IntPtr handle) :
            this(handle, null, true)
        {
        }

        /// <inheritdoc />
        public WindowInfo(IntPtr handle, WindowInfo parent) :
            this(handle, parent, true)
        {
        }

        /// <inheritdoc />
        public WindowInfo(IntPtr handle, bool getChildren) :
            this(handle, null, getChildren)
        {
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{$"{Handle.ToInt32():X16}".ToUpperInvariant()} '{ClassName}' '{Caption}' '{RawText}' " +
                   $"[{RectangleWindow.X}, {RectangleWindow.Y}; {RectangleWindow.Width} x {RectangleWindow.Height}] " +
                   $"[{RectangleClientAbsolute.X}, {RectangleClientAbsolute.Y}; {RectangleClientAbsolute.Width} x {RectangleClientAbsolute.Height}] " +
                   $"[{RectangleClientLocal.X}, {RectangleClientLocal.Y}; {RectangleClientLocal.Width} x {RectangleClientLocal.Height}]";
        }

        /// <summary>
        /// Collect children of this window.
        /// </summary>
        private void CollectChildren()
        {
            var child = IntPtr.Zero;
            while (true)
            {
                child = User32.FindWindowEx(Handle, child, null, null);
                if (child != IntPtr.Zero)
                {
                    Children.Add(new WindowInfo(child, this, true));
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Get root window handle.
        /// </summary>
        public static IntPtr GetRootHandle(IntPtr handle)
        {
            var rootHandle = handle;
            while (true)
            {
                var parentHandle = User32.GetParent(rootHandle);
                if (parentHandle != IntPtr.Zero)
                {
                    rootHandle = parentHandle;
                }
                else
                {
                    break;
                }
            }
            return rootHandle;
        }

        /// <summary>
        /// Get root <see cref="WindowInfo"/>.
        /// </summary>
        public WindowInfo GetRoot() => new WindowInfo(GetRootHandle(Handle));

        #endregion
    }
}
