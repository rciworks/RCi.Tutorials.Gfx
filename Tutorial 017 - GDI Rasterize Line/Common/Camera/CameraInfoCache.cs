using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Common.Camera
{
    /// <inheritdoc cref="ICameraInfoCache" />
    public class CameraInfoCache :
        ICameraInfoCache
    {
        #region // storage

        /// <inheritdoc />
        public Matrix4D MatrixView { get; }

        /// <inheritdoc />
        public Matrix4D MatrixViewInverse { get; }

        /// <inheritdoc />
        public Matrix4D MatrixProjection { get; }

        /// <inheritdoc />
        public Matrix4D MatrixProjectionInverse { get; }

        /// <inheritdoc />
        public Matrix4D MatrixViewport { get; }

        /// <inheritdoc />
        public Matrix4D MatrixViewportInverse { get; }

        /// <inheritdoc />
        public Matrix4D MatrixViewProjection { get; }

        /// <inheritdoc />
        public Matrix4D MatrixViewProjectionInverse { get; }

        /// <inheritdoc />
        public Matrix4D MatrixViewProjectionViewport { get; }

        /// <inheritdoc />
        public Matrix4D MatrixViewProjectionViewportInverse { get; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public CameraInfoCache(ICameraInfo cameraInfo)
        {
            // raw

            // world space -> camera space
            MatrixView = Matrix4DEx.LookAtRH(cameraInfo.Position.ToVector3D(), cameraInfo.Target.ToVector3D(), cameraInfo.UpVector);
            MatrixViewInverse = MatrixView.Inverse();

            // camera space -> clip space
            MatrixProjection = cameraInfo.Projection.GetMatrixProjection();
            MatrixProjectionInverse = MatrixProjection.Inverse();

            // clip space -> screen space
            MatrixViewport = Matrix4DEx.Viewport(cameraInfo.Viewport);
            MatrixViewportInverse = MatrixViewport.Inverse();

            // multiplicatives

            // world space -> camera space -> clip space
            MatrixViewProjection = MatrixView * MatrixProjection;
            MatrixViewProjectionInverse = MatrixViewProjection.Inverse();

            // world space -> camera space -> clip space -> screen space
            MatrixViewProjectionViewport = MatrixViewProjection * MatrixViewport;
            MatrixViewProjectionViewportInverse = MatrixViewProjectionViewport.Inverse();
        }

        #endregion
    }
}
