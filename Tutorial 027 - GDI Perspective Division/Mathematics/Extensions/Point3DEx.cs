using System.Runtime.CompilerServices;
using MathNet.Spatial.Euclidean;

namespace RCi.Tutorials.Gfx.Mathematics.Extensions
{
    public static class Point3DEx
    {
        #region // import

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3D(this Point2D value, double z) => new Point3D(value.X, value.Y, z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3D(this Vector2D value, double z) => new Point3D(value.X, value.Y, z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3D(this Vector2F value, double z) => new Point3D(value.X, value.Y, z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3D(this in Point3D value) => new Point3D(value.X, value.Y, value.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3D(this in Vector3D value) => new Point3D(value.X, value.Y, value.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3D(this in UnitVector3D value) => new Point3D(value.X, value.Y, value.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3D(this Vector3F value) => new Point3D(value.X, value.Y, value.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3D(this in Vector4D value) => new Point3D(value.X, value.Y, value.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3D(this Vector4F value) => new Point3D(value.X, value.Y, value.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3DNormalized(this in Vector4D value) => new Point3D(value.X / value.W, value.Y / value.W, value.Z / value.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3DNormalized(this Vector4F value) => new Point3D(value.X / value.W, value.Y / value.W, value.Z / value.W);

        #endregion

        #region // export

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] ToDoubles(this Point3D value) => new[] { value.X, value.Y, value.Z };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] ToFloats(this Point3D value) => new[] { (float)value.X, (float)value.Y, (float)value.Z };

        #endregion

        #region // interpolation

        public static Point3D InterpolateMultiply(this in Point3D value, double multiplier)
        {
            return new Point3D
            (
                value.X.InterpolateMultiply(multiplier),
                value.Y.InterpolateMultiply(multiplier),
                value.Z.InterpolateMultiply(multiplier)
            );
        }

        public static Point3D InterpolateLinear(this in Point3D value, in Point3D other, double alpha)
        {
            return new Point3D
            (
                value.X.InterpolateLinear(other.X, alpha),
                value.Y.InterpolateLinear(other.Y, alpha),
                value.Z.InterpolateLinear(other.Z, alpha)
            );
        }

        public static Point3D InterpolateBarycentric(this in Point3D value, in Point3D other0, in Point3D other1, Vector3D barycentric)
        {
            return new Point3D
            (
                value.X.InterpolateBarycentric(other0.X, other1.X, barycentric),
                value.Y.InterpolateBarycentric(other0.Y, other1.Y, barycentric),
                value.Z.InterpolateBarycentric(other0.Z, other1.Z, barycentric)
            );
        }

        #endregion
    }
}
