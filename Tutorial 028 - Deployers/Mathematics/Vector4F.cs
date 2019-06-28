using System;
using System.Runtime.InteropServices;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Mathematics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Vector4F :
        IInterpolateSingle<Vector4F>
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

        public Vector4F(float x, float y, float z, float w) => (X, Y, Z, W) = (x, y, z, w);

        #endregion

        #region // operators

        public static Vector4F operator +(Vector4F left, Vector4F right)
        {
            return new Vector4F
            (
                left.X + right.X,
                left.Y + right.Y,
                left.Z + right.Z,
                left.W + right.W
            );
        }

        public static Vector4F operator -(Vector4F left, Vector4F right)
        {
            return new Vector4F
            (
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z,
                left.W - right.W
            );
        }

        public static Vector4F operator *(Vector4F left, float right)
        {
            return new Vector4F
            (
                left.X * right,
                left.Y * right,
                left.Z * right,
                left.W * right
            );
        }

        public static Vector4F operator /(Vector4F left, float right)
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

        #region // interpolation

        public Vector4F InterpolateMultiply(float multiplier)
        {
            return new Vector4F
            (
                X.InterpolateMultiply(multiplier),
                Y.InterpolateMultiply(multiplier),
                Z.InterpolateMultiply(multiplier),
                W.InterpolateMultiply(multiplier)
            );
        }

        public Vector4F InterpolateLinear(in Vector4F other, float alpha)
        {
            return new Vector4F
            (
                X.InterpolateLinear(other.X, alpha),
                Y.InterpolateLinear(other.Y, alpha),
                Z.InterpolateLinear(other.Z, alpha),
                W.InterpolateLinear(other.W, alpha)
            );
        }

        public Vector4F InterpolateBarycentric(in Vector4F other0, in Vector4F other1, Vector3F barycentric)
        {
            return new Vector4F
            (
                X.InterpolateBarycentric(other0.X, other1.X, barycentric),
                Y.InterpolateBarycentric(other0.Y, other1.Y, barycentric),
                Z.InterpolateBarycentric(other0.Z, other1.Z, barycentric),
                W.InterpolateBarycentric(other0.W, other1.W, barycentric)
            );
        }

        #endregion

        #region // routines

        public override string ToString() => $"{X:0.000000}, {Y:0.000000}, {Z:0.000000}, {W:0.000000}";

        #endregion
    }
}
