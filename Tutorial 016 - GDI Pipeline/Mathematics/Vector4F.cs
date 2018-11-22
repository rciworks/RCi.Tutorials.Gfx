using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RCi.Tutorials.Gfx.Mathematics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Vector4F
    {
        #region // static

        public static readonly Vector4F Zero = new Vector4F(0, 0, 0, 0);

        #endregion

        #region // storage

        public float X { get; }

        public float Y { get; }

        public float Z { get; }

        public float W { get; }

        #endregion

        #region // ctor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4F(float x, float y, float z, float w) => (X, Y, Z, W) = (x, y, z, w);

        #endregion

        #region // operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4F operator +(in Vector4F left, in Vector4F right)
        {
            return new Vector4F
            (
                left.X + right.X,
                left.Y + right.Y,
                left.Z + right.Z,
                left.W + right.W
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4F operator -(in Vector4F left, in Vector4F right)
        {
            return new Vector4F
            (
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z,
                left.W - right.W
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4F operator *(in Vector4F left, float right)
        {
            return new Vector4F
            (
                left.X * right,
                left.Y * right,
                left.Z * right,
                left.W * right
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4F operator /(in Vector4F left, float right)
        {
            return new Vector4F
            (
                left.X / right,
                left.Y / right,
                left.Z / right,
                left.W / right
            );
        }

        #endregion

        #region // routines

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{X:0.000000}, {Y:0.000000}, {Z:0.000000}, {W:0.000000}";

        #endregion
    }
}
