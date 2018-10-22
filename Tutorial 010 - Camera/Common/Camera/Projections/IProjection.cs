using System;
using MathNet.Numerics.LinearAlgebra;

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
        Matrix<double> GetMatrixProjection();

        /// <summary>
        /// Create new projection based on existing one and adjusted by new aspect ratio.
        /// </summary>
        IProjection GetAdjustedProjection(double aspectRatio);
    }
}
