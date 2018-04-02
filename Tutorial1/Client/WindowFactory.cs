using System;
using System.Collections.Generic;
using System.Linq;
using RCi.Tutorials.Gfx.Engine.Render;
using RCi.Tutorials.Gfx.Utils;
using RCi.Tutorials.Gfx.Win;

namespace RCi.Tutorials.Gfx.Client
{
    public static class WindowFactory
    {
        /// <summary>
        /// Create various windows for rendering.
        /// </summary>
        public static IReadOnlyList<IRenderHost> SeedWindows()
        {
            // arbitrary default window size
            var size = new System.Drawing.Size(720, 480);

            // create windows (and render hosts)
            var renderHosts = new[]
            {
                CreateWindowForm(size, "Forms Gdi", h => new Drivers.Gdi.Render.RenderHost(h)),
                CreateWindowWpf(size, "Wpf Gdi", h => new Drivers.Gdi.Render.RenderHost(h)),
            };

            // sort windows in the middle of screen
            SortWindows(renderHosts);

            return renderHosts;
        }

        /// <summary>
        /// Create <see cref="System.Windows.Forms.Form"/> and <see cref="IRenderHost"/> for it.
        /// </summary>
        private static IRenderHost CreateWindowForm(System.Drawing.Size size, string title, Func<IntPtr, IRenderHost> ctorRenderHost)
        {
            var window = new System.Windows.Forms.Form
            {
                Size = size,
                Text = title,
            };

            var hostControl = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackColor = System.Drawing.Color.Transparent,
                ForeColor = System.Drawing.Color.Transparent,
            };
            window.Controls.Add(hostControl);

            // fix mouse wheel
            hostControl.MouseEnter += (sender, args) =>
            {
                if (System.Windows.Forms.Form.ActiveForm != window) window.Activate();
                if (!hostControl.Focused) hostControl.Focus();
            };

            window.Closed += (sender, args) => System.Windows.Application.Current.Shutdown();

            window.Show();

            return ctorRenderHost(hostControl.Handle());
        }

        /// <summary>
        /// Create <see cref="System.Windows.Window"/> and <see cref="IRenderHost"/> for it.
        /// </summary>
        private static IRenderHost CreateWindowWpf(System.Drawing.Size size, string title, Func<IntPtr, IRenderHost> ctorRenderHost)
        {
            var window = new System.Windows.Window
            {
                Width = size.Width,
                Height = size.Height,
                Title = title,
            };

            var hostControl = new System.Windows.Controls.Grid
            {
                Background = System.Windows.Media.Brushes.Transparent,
                Focusable = true,
            };
            window.Content = hostControl;

            // fix mouse wheel (although it's only for Windows.Forms, this is just OCD to make window active as well)
            hostControl.MouseEnter += (sender, args) =>
            {
                if (!window.IsActive) window.Activate();
                if (!hostControl.IsFocused) hostControl.Focus();
            };

            window.Closed += (sender, args) => System.Windows.Application.Current.Shutdown();

            window.Show();

            return ctorRenderHost(hostControl.Handle());
        }

        /// <summary>
        /// Sort windows in the middle of primary screen.
        /// </summary>
        private static void SortWindows(IEnumerable<IRenderHost> renderHosts)
        {
            // get window infos from handles
            var windowInfos = renderHosts.Select(renderHost => new WindowInfo(renderHost.HostHandle).GetRoot()).ToArray();

            // figure out max size of window
            var maxSize = new System.Drawing.Size(windowInfos.Max(a => a.RectangleWindow.Width), windowInfos.Max(a => a.RectangleWindow.Height));

            // get max columns and rows
            var maxColumns = (int)Math.Ceiling(Math.Sqrt(windowInfos.Length));
            var maxRows = (int)Math.Ceiling((double)windowInfos.Length / maxColumns);

            // get initial top left corner
            var primaryScreen = System.Windows.Forms.Screen.PrimaryScreen;
            var left = primaryScreen.WorkingArea.Width / 2 - maxColumns * maxSize.Width / 2;
            var top = primaryScreen.WorkingArea.Height / 2 - maxRows * maxSize.Height / 2;

            // try to move windows
            for (var row = 0; row < maxRows; row++)
            {
                for (var column = 0; column < maxColumns; column++)
                {
                    // figure out window index
                    var i = row * maxColumns + column;

                    // if it's too big, we moved all of them
                    if (i >= windowInfos.Length) return;

                    // get top left coordinate for window
                    var x = column * maxSize.Width + left;
                    var y = row * maxSize.Height + top;

                    // get window info
                    var windowInfo = windowInfos[i];

                    // move window
                    User32.MoveWindow(windowInfo.Handle, x, y, windowInfo.RectangleWindow.Width, windowInfo.RectangleWindow.Height, false);
                }
            }
        }
    }
}
