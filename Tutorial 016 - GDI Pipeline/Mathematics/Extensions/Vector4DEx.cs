using System.Runtime.CompilerServices;
using MathNet.Spatial.Euclidean;

namespace RCi.Tutorials.Gfx.Mathematics.Extensions
{
    public static class Vector4DEx
    {
        #region // from

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this in Vector2D value, double z, double w)
        {
            return new Vector4D(value.X, value.Y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this in Vector3D value, double w)
        {
            return new Vector4D(value.X, value.Y, value.Z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this in UnitVector3D value, double w)
        {
            return new Vector4D(value.X, value.Y, value.Z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this in Point2D value, double z, double w)
        {
            return new Vector4D(value.X, value.Y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this in Point3D value, double w)
        {
            return new Vector4D(value.X, value.Y, value.Z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this in Vector2F value, double z, double w)
        {
            return new Vector4D(value.X, value.Y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ToVector4D(this in Vector3F value, double w)
        {
            return new Vector4D(value.X, value.Y, value.Z, w);
        }

        #endregion

        #region // to

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D ToVector3DNormalized(this in Vector4D value)
        {
            return new Vector3D(value.X / value.W, value.Y / value.W, value.Z / value.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3DNormalized(this in Vector4D value)
        {
            return new Point3D(value.X / value.W, value.Y / value.W, value.Z / value.W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D ToVector3D(this in Vector4D value)
        {
            return new Vector3D(value.X, value.Y, value.Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D ToVector2D(this in Vector4D value)
        {
            return new Vector2D(value.X, value.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3D ToPoint3D(this in Vector4D value)
        {
            return new Point3D(value.X, value.Y, value.Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D ToPoint2D(this in Vector4D value)
        {
            return new Point2D(value.X, value.Y);
        }

        #endregion
    }
}
