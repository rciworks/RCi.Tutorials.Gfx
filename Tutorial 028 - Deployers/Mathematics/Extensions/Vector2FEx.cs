using MathNet.Spatial.Euclidean;

namespace RCi.Tutorials.Gfx.Mathematics.Extensions
{
    public static class Vector2FEx
    {
        #region // import

        public static Vector2F ToVector2F(this Point2D value) => new Vector2F((float)value.X, (float)value.Y);

        public static Vector2F ToVector2F(this Vector2D value) => new Vector2F((float)value.X, (float)value.Y);

        public static Vector2F ToVector2F(this Vector2F value) => new Vector2F(value.X, value.Y);

        public static Vector2F ToVector2F(this in Point3D value) => new Vector2F((float)value.X, (float)value.Y);

        public static Vector2F ToVector2F(this in Vector3D value) => new Vector2F((float)value.X, (float)value.Y);

        public static Vector2F ToVector2F(this in UnitVector3D value) => new Vector2F((float)value.X, (float)value.Y);

        public static Vector2F ToVector2F(this Vector3F value) => new Vector2F(value.X, value.Y);

        public static Vector2F ToVector2F(this in Vector4D value) => new Vector2F((float)value.X, (float)value.Y);

        public static Vector2F ToVector2F(this Vector4F value) => new Vector2F(value.X, value.Y);

        #endregion

        #region // export

        public static double[] ToDoubles(this Vector2F value) => new double[] { value.X, value.Y };

        public static float[] ToFloats(this Vector2F value) => new[] { value.X, value.Y };

        #endregion
    }
}
