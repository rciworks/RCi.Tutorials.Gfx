using MathNet.Spatial.Euclidean;

namespace RCi.Tutorials.Gfx.Mathematics
{
    #region // single

    /// <summary>
    /// Interpolation contract.
    /// </summary>
    public interface IInterpolateSingle<TValue> :
        IInterpolateSingleMultiply<TValue>,
        IInterpolateSingleLinear<TValue>,
        IInterpolateSingleBarycentric<TValue>
    {
    }

    /// <summary>
    /// Multiply interpolation contract.
    /// </summary>
    public interface IInterpolateSingleMultiply<out TValue>
    {
        /// <summary>
        /// Multiply by given multiplier.
        /// </summary>
        TValue InterpolateMultiply(float multiplier);
    }

    /// <summary>
    /// Linear interpolation contract.
    /// </summary>
    public interface IInterpolateSingleLinear<TValue>
    {
        /// <summary>
        /// Interpolate linearly with given alpha distance [0..1] to other instance.
        /// </summary>
        TValue InterpolateLinear(in TValue other, float alpha);
    }

    /// <summary>
    /// Barycentric interpolation contract.
    /// https://en.wikipedia.org/wiki/Barycentric_coordinate_system
    /// </summary>
    public interface IInterpolateSingleBarycentric<TValue>
    {
        /// <summary>
        /// Interpolate in between three components passing barycentric coordinates.
        /// </summary>
        TValue InterpolateBarycentric(in TValue other0, in TValue other1, Vector3F barycentric);
    }

    #endregion

    #region // double

    /// <summary>
    /// Interpolation contract.
    /// </summary>
    public interface IInterpolateDouble<TValue> :
        IInterpolateDoubleMultiply<TValue>,
        IInterpolateDoubleLinear<TValue>,
        IInterpolateDoubleBarycentric<TValue>
    {
    }

    /// <summary>
    /// Multiply interpolation contract.
    /// </summary>
    public interface IInterpolateDoubleMultiply<out TValue>
    {
        /// <summary>
        /// Multiply by given multiplier.
        /// </summary>
        TValue InterpolateMultiply(double multiplier);
    }

    /// <summary>
    /// Linear interpolation contract.
    /// </summary>
    public interface IInterpolateDoubleLinear<TValue>
    {
        /// <summary>
        /// Interpolate linearly with given alpha distance [0..1] to other instance.
        /// </summary>
        TValue InterpolateLinear(in TValue other, double alpha);
    }

    /// <summary>
    /// Barycentric interpolation contract.
    /// https://en.wikipedia.org/wiki/Barycentric_coordinate_system
    /// </summary>
    public interface IInterpolateDoubleBarycentric<TValue>
    {
        /// <summary>
        /// Interpolate in between three components passing barycentric coordinates.
        /// </summary>
        TValue InterpolateBarycentric(in TValue other0, in TValue other1, Vector3D barycentric);
    }

    #endregion
}
