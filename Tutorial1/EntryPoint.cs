using System;

namespace RCi.Tutorials.Gfx
{
    internal class EntryPoint
    {
        [STAThread]
        private static void Main() => new Client.Program().Run();
    }
}
