using System.Runtime.CompilerServices;
using MathNet.Spatial.Euclidean;

namespace RCi.Tutorials.Gfx.Mathematics.Extensions
{
    public static class Vector4DEx
    {
        #region // import

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this Point2D value, double z, double w) => new Vector4D(value.X, value.Y, z, w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this Vector2D value, double z, double w) => new Vector4D(value.X, value.Y, z, w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this Vector2F value, double z, double w) => new Vector4D(value.X, value.Y, z, w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this in Point3D value, double w) => new Vector4D(value.X, value.Y, value.Z, w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this in Vector3D value, double w) => new Vector4D(value.X, value.Y, value.Z, w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this in UnitVector3D value, double w) => new Vector4D(value.X, value.Y, value.Z, w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this Vector3F value, double w) => new Vector4D(value.X, value.Y, value.Z, w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this in Vector4D value) => new Vector4D(value.X, value.Y, value.Z, value.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this Vector4F value) => new Vector4D(value.X, value.Y, value.Z, value.W);

        #endregion

        #region // export

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] ToDoubles(this Vector4D value) => new[] { value.X, value.Y, value.Z, value.W };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] ToFloats(this Vector4D value) => new[] { (float)value.X, (float)value.Y, (float)value.Z, (float)value.W };

        #endregion
    }
}
