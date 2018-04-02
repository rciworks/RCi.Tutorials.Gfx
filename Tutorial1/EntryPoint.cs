using System;

namespace RCi.Tutorials.Gfx
{
    /// <summary>
    /// Entry point class.
    /// </summary>
    internal class EntryPoint
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        [STAThread]
        private static void Main() => new Client.Program().Run();
    }
}
