using MathNet.Numerics.LinearAlgebra;

namespace RCi.Tutorials.Gfx.Common.Camera.Projections
{
    /// <inheritdoc cref="IProjection"/>
    public abstract class Projection :
        IProjection
    {
        #region // storage

        /// <inheritdoc />
        public double NearPlane { get; }

        /// <inheritdoc />
        public double FarPlane { get; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        protected Projection(double nearPlane, double farPlane)
        {
            NearPlane = nearPlane;
            FarPlane = farPlane;
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        public abstract object Clone();

        /// <inheritdoc />
        public abstract Matrix<double> GetMatrixProjection();

        /// <inheritdoc />
        public abstract IProjection GetAdjustedProjection(double aspectRatio);

        #endregion
    }
}
