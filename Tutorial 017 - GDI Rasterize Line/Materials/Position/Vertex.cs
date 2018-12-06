using System.Runtime.InteropServices;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Materials.Position
{
    /// <inheritdoc cref="IVertex"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex :
        IVertex
    {
        #region // storage

        /// <inheritdoc />
        public Vector3F Position { get; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Vertex(Vector3F position)
        {
            Position = position;
        }

        #endregion
    }
}
