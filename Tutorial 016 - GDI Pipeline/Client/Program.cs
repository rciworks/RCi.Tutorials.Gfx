using System;
using System.Collections.Generic;
using RCi.Tutorials.Gfx.Engine.Render;
using RCi.Tutorials.Gfx.Materials;
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
                Render(RenderHosts, Seed.GetPrimitives());

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
        private static void Render(IEnumerable<IRenderHost> renderHosts, IEnumerable<IPrimitive> primitives)
        {
            renderHosts.ForEach(rh => rh.Render(primitives));
        }

        #endregion
    }
}
