using MathNet.Spatial.Euclidean;

namespace RCi.Tutorials.Gfx.Mathematics.Extensions
{
    public static class UnitVector3DEx
    {
        #region // interpolation

        public static UnitVector3D InterpolateMultiply(this in UnitVector3D value, double multiplier)
        {
            return new UnitVector3D
            (
                value.X.InterpolateMultiply(multiplier),
                value.Y.InterpolateMultiply(multiplier),
                value.Z.InterpolateMultiply(multiplier)
            );
        }

        public static UnitVector3D InterpolateLinear(this in UnitVector3D value, in UnitVector3D other, double alpha)
        {
            return new UnitVector3D
            (
                value.X.InterpolateLinear(other.X, alpha),
                value.Y.InterpolateLinear(other.Y, alpha),
                value.Z.InterpolateLinear(other.Z, alpha)
            );
        }

        public static UnitVector3D InterpolateBarycentric(this in UnitVector3D value, in UnitVector3D other0, in UnitVector3D other1, Vector3D barycentric)
        {
            return new UnitVector3D
            (
                value.X.InterpolateBarycentric(other0.X, other1.X, barycentric),
                value.Y.InterpolateBarycentric(other0.Y, other1.Y, barycentric),
                value.Z.InterpolateBarycentric(other0.Z, other1.Z, barycentric)
            );
        }

        #endregion
    }
}