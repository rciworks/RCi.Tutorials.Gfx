using System;
using System.Collections.Generic;
using RCi.Tutorials.Gfx.Engine.Render;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Client
{
    internal class Program :
        System.Windows.Application,
        IDisposable
    {
        #region // storage

        private IReadOnlyList<IRenderHost> RenderHosts { get; set; }

        #endregion

        #region // ctor

        public Program()
        {
            Startup += (sender, args) => Ctor();
            Exit += (sender, args) => Dispose();
        }

        private void Ctor()
        {
            RenderHosts = WindowFactory.SeedWindows();
        }

        public void Dispose()
        {
            RenderHosts.ForEach(host => host.Dispose());
            RenderHosts = default;
        }

        #endregion
    }
}
