using System;
using System.Runtime.InteropServices;
using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Mathematics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Vector4D :
        IInterpolateDouble<Vector4D>
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

        public Vector4D(double x, double y, double z, double w) => (X, Y, Z, W) = (x, y, z, w);

        #endregion

        #region // operators

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

        public override string ToString() => $"{X:0.000000}, {Y:0.000000}, {Z:0.000000}, {W:0.000000}";

        #endregion

        #region // interpolation

        public Vector4D InterpolateMultiply(double multiplier)
        {
            return new Vector4D
            (
                X.InterpolateMultiply(multiplier),
                Y.InterpolateMultiply(multiplier),
                Z.InterpolateMultiply(multiplier),
                W.InterpolateMultiply(multiplier)
            );
        }

        public Vector4D InterpolateLinear(in Vector4D other, double alpha)
        {
            return new Vector4D
            (
                X.InterpolateLinear(other.X, alpha),
                Y.InterpolateLinear(other.Y, alpha),
                Z.InterpolateLinear(other.Z, alpha),
                W.InterpolateLinear(other.W, alpha)
            );
        }

        public Vector4D InterpolateBarycentric(in Vector4D other0, in Vector4D other1, Vector3D barycentric)
        {
            return new Vector4D
            (
                X.InterpolateBarycentric(other0.X, other1.X, barycentric),
                Y.InterpolateBarycentric(other0.Y, other1.Y, barycentric),
                Z.InterpolateBarycentric(other0.Z, other1.Z, barycentric),
                W.InterpolateBarycentric(other0.W, other1.W, barycentric)
            );
        }

        #endregion
    }
}
