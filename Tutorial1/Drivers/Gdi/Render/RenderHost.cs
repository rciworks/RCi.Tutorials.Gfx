using System;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render
{
    public class RenderHost
        : Engine.Render.RenderHost
    {
        public RenderHost(IntPtr hostHandle) :
            base(hostHandle)
        {
        }
    }
}
