using System.Runtime.CompilerServices;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.Position
{
    /// <inheritdoc cref="IVertexShader"/>
    public readonly struct VertexShader :
        IVertexShader<VertexShader>
    {
        #region // storage

        /// <inheritdoc />
        public Vector4F Position { get; }

        #endregion

        #region // ctor

        /// <summary />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VertexShader(Vector4F position)
        {
            Position = position;
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VertexShader CloneWithNewPosition(Vector4F position)
        {
            return new VertexShader(position);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"Position: {Position}";
        }

        #endregion
    }
}
