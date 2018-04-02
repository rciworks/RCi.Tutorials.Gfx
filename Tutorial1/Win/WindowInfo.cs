using System;
using System.Collections.Generic;

namespace RCi.Tutorials.Gfx.Win
{
    public class WindowInfo
    {
        #region // storage

        public IntPtr Handle { get; }
        public WindowInfo Parent { get; }
        public IntPtr ParentHandle { get; }
        public string ClassName { get; }
        public string Caption { get; }
        public string RawText { get; }
        public System.Drawing.Rectangle RectangleWindow { get; }
        public System.Drawing.Rectangle RectangleClientLocal { get; }
        public System.Drawing.Rectangle RectangleClientAbsolute { get; }
        public bool IsWindowVisible { get; }
        public List<WindowInfo> Children { get; }

        #endregion

        #region // ctor

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

        public WindowInfo(IntPtr handle) :
            this(handle, null, true)
        {
        }

        public WindowInfo(IntPtr handle, WindowInfo parent) :
            this(handle, parent, true)
        {
        }

        public WindowInfo(IntPtr handle, bool getChildren) :
            this(handle, null, getChildren)
        {
        }

        #endregion

        #region // routines

        public override string ToString()
        {
            return $"{$"{Handle.ToInt32():X16}".ToUpperInvariant()} '{ClassName}' '{Caption}' '{RawText}' " +
                   $"[{RectangleWindow.X}, {RectangleWindow.Y}; {RectangleWindow.Width} x {RectangleWindow.Height}] " +
                   $"[{RectangleClientAbsolute.X}, {RectangleClientAbsolute.Y}; {RectangleClientAbsolute.Width} x {RectangleClientAbsolute.Height}] " +
                   $"[{RectangleClientLocal.X}, {RectangleClientLocal.Y}; {RectangleClientLocal.Width} x {RectangleClientLocal.Height}]";
        }

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

        public WindowInfo GetRoot()
        {
            var root = this;
            while (root.ParentHandle != IntPtr.Zero)
            {
                root = new WindowInfo(root.ParentHandle);
            }
            return root;
        }

        #endregion
    }
}
