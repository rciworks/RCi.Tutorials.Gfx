using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Common.Camera.Projections
{
    /// <inheritdoc cref="IProjectionCombined"/>
    public class ProjectionCombined :
        IProjectionCombined
    {
        #region // routines

        /// <inheritdoc />
        public IProjection Projection0 { get; }

        /// <inheritdoc />
        public IProjection Projection1 { get; }

        /// <inheritdoc />
        public double Weight0 { get; }

        /// <inheritdoc />
        public double Weight1 { get; }

        /// <inheritdoc />
        public double NearPlane => Projection0.NearPlane.InterpolateLinear(Projection1.NearPlane, Weight1);

        /// <inheritdoc />
        public double FarPlane => Projection0.FarPlane.InterpolateLinear(Projection1.FarPlane, Weight1);

        #endregion

        #region // ctor

        /// <summary />
        public ProjectionCombined(IProjection projection0, IProjection projection1, double weight0)
        {
            Projection0 = projection0;
            Projection1 = projection1;
            Weight0 = weight0;
            Weight1 = 1 - Weight0;
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        public object Clone()
        {
            return new ProjectionCombined(Projection0, Projection1, Weight0);
        }

        /// <inheritdoc />
        public Matrix4D GetMatrixProjection()
        {
            return Projection0.GetMatrixProjection().InterpolateLinear(Projection1.GetMatrixProjection(), Weight1);
        }

        /// <inheritdoc />
        public IProjection GetAdjustedProjection(double aspectRatio)
        {
            return new ProjectionCombined(Projection0.GetAdjustedProjection(aspectRatio), Projection1.GetAdjustedProjection(aspectRatio), Weight0);
        }

        /// <inheritdoc />
        public Ray3D GetMouseRay(ICameraInfo cameraInfo, Point3D mouseWorld)
        {
            var mouseRay0 = Projection0.GetMouseRay(cameraInfo, mouseWorld);
            var mouseRay1 = Projection1.GetMouseRay(cameraInfo, mouseWorld);

            var plane = new Plane(cameraInfo.Position, (cameraInfo.Target - cameraInfo.Position).Normalize());
            var rayOrigin0 = plane.IntersectionWith(mouseRay0);
            var rayOrigin1 = plane.IntersectionWith(mouseRay1);

            var rayOriginCombined = rayOrigin0.InterpolateLinear(rayOrigin1, Weight1);
            var rayDirectionCombined = mouseRay0.Direction.InterpolateLinear(mouseRay1.Direction, Weight1);

            return new Ray3D(rayOriginCombined, rayDirectionCombined);
        }

        #endregion
    }
}
