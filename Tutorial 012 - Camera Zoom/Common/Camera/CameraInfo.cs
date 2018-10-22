using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Common.Camera.Projections;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Common.Camera
{
    /// <inheritdoc cref="ICameraInfo"/>
    public class CameraInfo :
        ICameraInfo
    {
        #region // storage

        /// <inheritdoc />
        public Point3D Position { get; }

        /// <inheritdoc />
        public Point3D Target { get; }

        /// <inheritdoc />
        public UnitVector3D UpVector { get; }

        /// <inheritdoc />
        public IProjection Projection { get; }

        /// <inheritdoc />
        public Viewport Viewport { get; }

        /// <inheritdoc />
        public ICameraInfoCache Cache { get; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public CameraInfo(in Point3D position, in Point3D target, in UnitVector3D upVector, in IProjection projection, in Viewport viewport)
        {
            Position = position;
            Target = target;
            UpVector = upVector;
            Projection = projection;
            Viewport = viewport;
            Cache = new CameraInfoCache(this);
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        public object Clone()
        {
            return new CameraInfo(Position, Target, UpVector, Projection.Cloned(), Viewport);
        }

        #endregion
    }
}
