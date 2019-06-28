using System;
using System.Runtime.InteropServices;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Mathematics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Vector2F :
        IInterpolate<Vector2F>
    {
        #region // static

        public static readonly Vector2F Zero = new Vector2F(0, 0);

        #endregion

        #region // storage

        public float X { get; }

        public float Y { get; }

        #endregion

        #region // ctor

        public Vector2F(float x, float y) => (X, Y) = (x, y);

        #endregion

        #region // operators

        public static Vector2F operator +(Vector2F left, Vector2F right)
        {
            return new Vector2F
            (
                left.X + right.X,
                left.Y + right.Y
            );
        }

        public static Vector2F operator -(Vector2F left, Vector2F right)
        {
            return new Vector2F
            (
                left.X - right.X,
                left.Y - right.Y
            );
        }

        public static Vector2F operator *(Vector2F left, float right)
        {
            return new Vector2F
            (
                left.X * right,
                left.Y * right
            );
        }

        public static Vector2F operator /(Vector2F left, float right)
        {
            return new Vector2F
            (
                left.X / right,
                left.Y / right
            );
        }

        #endregion

        #region // interpolation

        public Vector2F InterpolateMultiply(float multiplier)
        {
            return new Vector2F
            (
                X.InterpolateMultiply(multiplier),
                Y.InterpolateMultiply(multiplier)
            );
        }

        public Vector2F InterpolateLinear(in Vector2F other, float alpha)
        {
            return new Vector2F
            (
                X.InterpolateLinear(other.X, alpha),
                Y.InterpolateLinear(other.Y, alpha)
            );
        }

        public Vector2F InterpolateBarycentric(in Vector2F other0, in Vector2F other1, Vector3F barycentric)
        {
            return new Vector2F
            (
                X.InterpolateBarycentric(other0.X, other1.X, barycentric),
                Y.InterpolateBarycentric(other0.Y, other1.Y, barycentric)
            );
        }

        #endregion

        #region // routines

        public override string ToString() => $"{X:0.000000}, {Y:0.000000}";

        #endregion
    }
}
