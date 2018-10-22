using System;
using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Common.Camera.Projections;

namespace RCi.Tutorials.Gfx.Common.Camera
{
    /// <summary>
    /// Camera information describing view.
    /// </summary>
    public interface ICameraInfo :
        ICloneable
    {
        /// <summary>
        /// Camera position.
        /// </summary>
        Point3D Position { get; }

        /// <summary>
        /// Camera target.
        /// </summary>
        Point3D Target { get; }

        /// <summary>
        /// Camera up vector.
        /// </summary>
        UnitVector3D UpVector { get; }

        /// <summary>
        /// Camera projection.
        /// </summary>
        IProjection Projection { get; }

        /// <summary>
        /// Camera viewport.
        /// </summary>
        Viewport Viewport { get; }

        /// <inheritdoc cref="ICameraInfoCache"/>
        ICameraInfoCache Cache { get; }
    }
}
