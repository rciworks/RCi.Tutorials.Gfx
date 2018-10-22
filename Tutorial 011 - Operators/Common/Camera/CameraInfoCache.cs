using MathNet.Numerics.LinearAlgebra;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Common.Camera
{
    /// <inheritdoc cref="ICameraInfoCache" />
    public class CameraInfoCache :
        ICameraInfoCache
    {
        #region // storage

        /// <inheritdoc />
        public Matrix<double> MatrixView { get; }

        /// <inheritdoc />
        public Matrix<double> MatrixViewInverse { get; }

        /// <inheritdoc />
        public Matrix<double> MatrixProjection { get; }

        /// <inheritdoc />
        public Matrix<double> MatrixProjectionInverse { get; }

        /// <inheritdoc />
        public Matrix<double> MatrixViewport { get; }

        /// <inheritdoc />
        public Matrix<double> MatrixViewportInverse { get; }

        /// <inheritdoc />
        public Matrix<double> MatrixViewProjection { get; }

        /// <inheritdoc />
        public Matrix<double> MatrixViewProjectionInverse { get; }

        /// <inheritdoc />
        public Matrix<double> MatrixViewProjectionViewport { get; }

        /// <inheritdoc />
        public Matrix<double> MatrixViewProjectionViewportInverse { get; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public CameraInfoCache(ICameraInfo cameraInfo)
        {
            // raw

            // world space -> camera space
            MatrixView = MatrixEx.LookAtRH(cameraInfo.Position.ToVector3D(), cameraInfo.Target.ToVector3D(), cameraInfo.UpVector);
            MatrixViewInverse = MatrixView.Inverse();

            // camera space -> clip space
            MatrixProjection = cameraInfo.Projection.GetMatrixProjection();
            MatrixProjectionInverse = MatrixProjection.Inverse();

            // clip space -> screen space
            MatrixViewport = MatrixEx.Viewport(cameraInfo.Viewport);
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
