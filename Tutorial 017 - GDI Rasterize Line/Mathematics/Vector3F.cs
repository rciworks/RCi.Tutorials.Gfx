using System;
using System.Runtime.InteropServices;

namespace RCi.Tutorials.Gfx.Mathematics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Vector3F
    {
        #region // static

        public static readonly Vector3F Zero = new Vector3F(0, 0, 0);

        #endregion

        #region // storage

        public float X { get; }

        public float Y { get; }

        public float Z { get; }

        #endregion

        #region // ctor

        public Vector3F(float x, float y, float z) => (X, Y, Z) = (x, y, z);

        #endregion

        #region // operators

        public static Vector3F operator +(in Vector3F left, in Vector3F right)
        {
            return new Vector3F
            (
                left.X + right.X,
                left.Y + right.Y,
                left.Z + right.Z
            );
        }

        public static Vector3F operator -(in Vector3F left, in Vector3F right)
        {
            return new Vector3F
            (
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z
            );
        }

        public static Vector3F operator *(in Vector3F left, float right)
        {
            return new Vector3F
            (
                left.X * right,
                left.Y * right,
                left.Z * right
            );
        }

        public static Vector3F operator /(in Vector3F left, float right)
        {
            return new Vector3F
            (
                left.X / right,
                left.Y / right,
                left.Z / right
            );
        }

        #endregion

        #region // routines

        public override string ToString() => $"{X:0.000000}, {Y:0.000000}, {Z:0.000000}";

        #endregion
    }
}
