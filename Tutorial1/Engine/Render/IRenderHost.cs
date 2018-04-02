using System;

namespace RCi.Tutorials.Gfx.Engine.Render
{
    public interface IRenderHost
        : IDisposable
    {
        IntPtr HostHandle { get; }
    }
}
