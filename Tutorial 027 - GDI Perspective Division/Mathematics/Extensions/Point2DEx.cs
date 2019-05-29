using System.Runtime.CompilerServices;
using MathNet.Spatial.Euclidean;

namespace RCi.Tutorials.Gfx.Mathematics.Extensions
{
    public static class Point2DEx
    {
        #region // import

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D ToPoint2D(this System.Drawing.Point value) => new Point2D(value.X, value.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D ToPoint2D(this System.Windows.Point value) => new Point2D(value.X, value.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D ToPoint2D(this Point2D value) => new Point2D(value.X, value.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D ToPoint2D(this Vector2D value) => new Point2D(value.X, value.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D ToPoint2D(this Vector2F value) => new Point2D(value.X, value.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D ToPoint2D(this in Point3D value) => new Point2D(value.X, value.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D ToPoint2D(this in Vector3D value) => new Point2D(value.X, value.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D ToPoint2D(this in UnitVector3D value) => new Point2D(value.X, value.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D ToPoint2D(this Vector3F value) => new Point2D(value.X, value.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D ToPoint2D(this in Vector4D value) => new Point2D(value.X, value.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D ToPoint2D(this Vector4F value) => new Point2D(value.X, value.Y);

        #endregion

        #region // export

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] ToDoubles(this Point2D value) => new[] { value.X, value.Y };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] ToFloats(this Point2D value) => new[] { (float)value.X, (float)value.Y };

        #endregion

        #region // interpolation

        public static Point2D InterpolateMultiply(this in Point2D value, double multiplier)
        {
            return new Point2D
            (
                value.X.InterpolateMultiply(multiplier),
                value.Y.InterpolateMultiply(multiplier)
            );
        }

        public static Point2D InterpolateLinear(this in Point2D value, in Point2D other, double alpha)
        {
            return new Point2D
            (
                value.X.InterpolateLinear(other.X, alpha),
                value.Y.InterpolateLinear(other.Y, alpha)
            );
        }

        public static Point2D InterpolateBarycentric(this in Point2D value, in Point2D other0, in Point2D other1, Vector3D barycentric)
        {
            return new Point2D
            (
                value.X.InterpolateBarycentric(other0.X, other1.X, barycentric),
                value.Y.InterpolateBarycentric(other0.Y, other1.Y, barycentric)
            );
        }

        #endregion
    }
}
