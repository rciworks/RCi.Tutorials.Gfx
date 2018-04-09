using System;
using System.Drawing;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render
{
    /// <summary>
    /// Gdi render host.
    /// </summary>
    public class RenderHost :
        Engine.Render.RenderHost
    {
        #region // storage

        private Graphics GraphicsHost { get; set; }

        private Font FontConsolas12 { get; set; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RenderHost(IntPtr hostHandle) :
            base(hostHandle)
        {
            GraphicsHost = Graphics.FromHwnd(HostHandle);

            FontConsolas12 = new Font("Consolas", 12);
        }

        public override void Dispose()
        {
            FontConsolas12?.Dispose();
            FontConsolas12 = default;

            GraphicsHost?.Dispose();
            GraphicsHost = default;

            base.Dispose();
        }

        #endregion

        #region // render

        protected override void RenderInternal()
        {
            GraphicsHost.Clear(Color.Black);
            GraphicsHost.DrawString(FpsCounter.FpsString, FontConsolas12, Brushes.Red, 0, 0);
        }

        #endregion
    }
}
