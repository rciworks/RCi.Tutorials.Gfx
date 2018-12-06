using RCi.Tutorials.Gfx.Mathematics;

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
        Matrix4D MatrixView { get; }

        /// <summary>
        /// Inverse <see cref="MatrixView"/>.
        /// </summary>
        Matrix4D MatrixViewInverse { get; }

        /// <summary>
        /// Matrix: camera space -> clip space.
        /// </summary>
        Matrix4D MatrixProjection { get; }

        /// <summary>
        /// Inverse <see cref="MatrixProjection"/>.
        /// </summary>
        Matrix4D MatrixProjectionInverse { get; }

        /// <summary>
        /// Matrix: clip space -> screen space.
        /// </summary>
        Matrix4D MatrixViewport { get; }

        /// <summary>
        /// Inverse <see cref="MatrixViewport"/>.
        /// </summary>
        Matrix4D MatrixViewportInverse { get; }

        /// <summary>
        /// Matrix: world space -> camera space -> clip space.
        /// </summary>
        Matrix4D MatrixViewProjection { get; }

        /// <summary>
        /// Inverse <see cref="MatrixViewProjection"/>.
        /// </summary>
        Matrix4D MatrixViewProjectionInverse { get; }

        /// <summary>
        /// Matrix: world space -> camera space -> clip space -> screen space.
        /// </summary>
        Matrix4D MatrixViewProjectionViewport { get; }

        /// <summary>
        /// Inverse <see cref="MatrixViewProjectionViewport"/>.
        /// </summary>
        Matrix4D MatrixViewProjectionViewportInverse { get; }
    }
}
