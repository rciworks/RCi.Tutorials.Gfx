using System;
using System.Collections.Generic;
using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Common.Camera;
using RCi.Tutorials.Gfx.Engine.Render;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Client
{
    /// <summary>
    /// Program for client side to use graphics modules.
    /// </summary>
    internal class Program :
        System.Windows.Application,
        IDisposable
    {
        #region // storage

        /// <summary>
        /// Our created render hosts.
        /// </summary>
        private IReadOnlyList<IRenderHost> RenderHosts { get; set; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Program()
        {
            // call actual constructor once our application is initialized
            Startup += (sender, args) => Ctor();

            // dispose resources when application is exiting
            Exit += (sender, args) => Dispose();
        }

        /// <summary>
        /// Functional constructor.
        /// </summary>
        private void Ctor()
        {
            // create some render hosts for rendering
            RenderHosts = WindowFactory.SeedWindows();

            // render loop
            while (!Dispatcher.HasShutdownStarted)
            {
                DebugCameras(RenderHosts);

                Render(RenderHosts);

                // message pump
                System.Windows.Forms.Application.DoEvents();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // dispose all render hosts we have
            RenderHosts.ForEach(host => host.Dispose());
            RenderHosts = default;
        }

        #endregion

        #region // routines

        /// <summary>
        /// Render <see cref="IRenderHost"/>s.
        /// </summary>
        private static void Render(IEnumerable<IRenderHost> renderHosts)
        {
            renderHosts.ForEach(rh => rh.Render());
        }

        private static void DebugCameras(IReadOnlyList<IRenderHost> renderHosts)
        {
            var utcNow = DateTime.UtcNow;
            const int radius = 2;

            for (var i = 0; i < renderHosts.Count; i++)
            {
                var t = Drivers.Gdi.Render.RenderHost.GetDeltaTime(utcNow, new TimeSpan(0, 0, 0, i % 2 == 0 ? 10 : 30));
                var angle = t * Math.PI * 2;
                angle *= i % 2 == 0 ? 1 : -1;

                var cameraInfo = renderHosts[i].CameraInfo;
                renderHosts[i].CameraInfo = new CameraInfo
                (
                    new Point3D(Math.Sin(angle) * radius, Math.Cos(angle) * radius, 1),
                    new Point3D(0, 0, 0),
                    cameraInfo.UpVector,
                    cameraInfo.Projection,
                    cameraInfo.Viewport
                );
            }
        }

        #endregion
    }
}
