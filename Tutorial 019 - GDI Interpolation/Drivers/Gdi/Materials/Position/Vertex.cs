using System.Runtime.CompilerServices;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.Position
{
    /// <inheritdoc cref="IVertex"/>
    public readonly struct Vertex :
        IVertex
    {
        #region // storage

        /// <inheritdoc />
        public Vector4F Position { get; }

        #endregion

        #region // ctor

        /// <summary />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vertex(Vector4F position)
        {
            Position = position;
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"Position: {Position}";
        }

        #endregion
    }
}
