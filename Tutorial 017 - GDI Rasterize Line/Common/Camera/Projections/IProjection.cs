using System;
using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Common.Camera.Projections
{
    /// <summary>
    /// View projection.
    /// </summary>
    public interface IProjection :
        ICloneable
    {
        /// <summary>
        /// Z-value of the near view-plane.
        /// </summary>
        double NearPlane { get; }

        /// <summary>
        /// Z-value of the far view-plane.
        /// </summary>
        double FarPlane { get; }

        /// <summary>
        /// Create projection matrix.
        /// </summary>
        Matrix4D GetMatrixProjection();

        /// <summary>
        /// Create new projection based on existing one and adjusted by new aspect ratio.
        /// </summary>
        IProjection GetAdjustedProjection(double aspectRatio);

        /// <summary>
        /// Get mouse ray in world space.
        /// </summary>
        Ray3D GetMouseRay(ICameraInfo cameraInfo, Point3D mouseWorld);
    }
}
