using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RCi.Tutorials.Gfx.Mathematics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Vector4D
    {
        #region // static

        public static readonly Vector4D Zero = new Vector4D(0, 0, 0, 0);

        #endregion

        #region // storage

        public double X { get; }

        public double Y { get; }

        public double Z { get; }

        public double W { get; }

        #endregion

        #region // ctor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4D(double x, double y, double z, double w) => (X, Y, Z, W) = (x, y, z, w);

        #endregion

        #region // operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator +(in Vector4D left, in Vector4D right)
        {
            return new Vector4D
            (
                left.X + right.X,
                left.Y + right.Y,
                left.Z + right.Z,
                left.W + right.W
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator -(in Vector4D left, in Vector4D right)
        {
            return new Vector4D
            (
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z,
                left.W - right.W
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator *(in Vector4D left, double right)
        {
            return new Vector4D
            (
                left.X * right,
                left.Y * right,
                left.Z * right,
                left.W * right
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator /(in Vector4D left, double right)
        {
            return new Vector4D
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
