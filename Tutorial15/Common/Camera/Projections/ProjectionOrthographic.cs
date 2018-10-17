using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Common.Camera.Projections
{
    /// <inheritdoc cref="IProjectionOrthographic"/>
    public class ProjectionOrthographic :
        Projection,
        IProjectionOrthographic
    {
        #region // storage

        /// <inheritdoc />
        public double FieldWidth { get; }

        /// <inheritdoc />
        public double FieldHeight { get; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public ProjectionOrthographic(double nearPlane, double farPlane, double fieldWidth, double fieldHeight) :
            base(nearPlane, farPlane)
        {
            FieldWidth = fieldWidth;
            FieldHeight = fieldHeight;
        }

        /// <summary>
        /// Create orthographic projection from distance between camera position and target.
        /// </summary>
        public static IProjectionOrthographic FromDistance(double nearPlane, double farPlane, double cameraPositionToTargetDistance, double aspectRatio)
        {
            return new ProjectionOrthographic(nearPlane, farPlane, cameraPositionToTargetDistance * aspectRatio, cameraPositionToTargetDistance);
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        public override object Clone()
        {
            return new ProjectionOrthographic(NearPlane, FarPlane, FieldWidth, FieldHeight);
        }

        /// <inheritdoc />
        public override Matrix<double> GetMatrixProjection()
        {
            return MatrixEx.OrthoRH(FieldWidth, FieldHeight, NearPlane, FarPlane);
        }

        /// <inheritdoc />
        public override IProjection GetAdjustedProjection(double aspectRatio)
        {
            return new ProjectionOrthographic(NearPlane, FarPlane, FieldHeight * aspectRatio, FieldHeight);
        }

        /// <inheritdoc />
        public override Ray3D GetMouseRay(ICameraInfo cameraInfo, Point3D mouseWorld)
        {
            return new Ray3D(mouseWorld, cameraInfo.GetEyeDirection());
        }

        #endregion
    }
}
