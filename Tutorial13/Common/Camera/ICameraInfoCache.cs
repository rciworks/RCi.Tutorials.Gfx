using MathNet.Numerics.LinearAlgebra;

namespace RCi.Tutorials.Gfx.Common.Camera
{
    /// <summary>
    /// Storage for matrices.
    /// </summary>
    public interface ICameraInfoCache
    {
        /// <summary>
        /// Matrix: world space -> camera space.
        /// </summary>
        Matrix<double> MatrixView { get; }

        /// <summary>
        /// Inverse <see cref="MatrixView"/>.
        /// </summary>
        Matrix<double> MatrixViewInverse { get; }

        /// <summary>
        /// Matrix: camera space -> clip space.
        /// </summary>
        Matrix<double> MatrixProjection { get; }

        /// <summary>
        /// Inverse <see cref="MatrixProjection"/>.
        /// </summary>
        Matrix<double> MatrixProjectionInverse { get; }

        /// <summary>
        /// Matrix: clip space -> screen space.
        /// </summary>
        Matrix<double> MatrixViewport { get; }

        /// <summary>
        /// Inverse <see cref="MatrixViewport"/>.
        /// </summary>
        Matrix<double> MatrixViewportInverse { get; }

        /// <summary>
        /// Matrix: world space -> camera space -> clip space.
        /// </summary>
        Matrix<double> MatrixViewProjection { get; }

        /// <summary>
        /// Inverse <see cref="MatrixViewProjection"/>.
        /// </summary>
        Matrix<double> MatrixViewProjectionInverse { get; }

        /// <summary>
        /// Matrix: world space -> camera space -> clip space -> screen space.
        /// </summary>
        Matrix<double> MatrixViewProjectionViewport { get; }

        /// <summary>
        /// Inverse <see cref="MatrixViewProjectionViewport"/>.
        /// </summary>
        Matrix<double> MatrixViewProjectionViewportInverse { get; }
    }
}
