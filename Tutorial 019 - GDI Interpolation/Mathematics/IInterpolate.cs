namespace RCi.Tutorials.Gfx.Mathematics
{
    /// <summary>
    /// Interpolation contract.
    /// </summary>
    public interface IInterpolate<TValue> :
        IInterpolateMultiply<TValue>,
        IInterpolateLinear<TValue>,
        IInterpolateBarycentric<TValue>
    {
    }

    /// <summary>
    /// Multiply interpolation contract.
    /// </summary>
    public interface IInterpolateMultiply<out TValue>
    {
        /// <summary>
        /// Multiply by given multiplier.
        /// </summary>
        TValue InterpolateMultiply(float multiplier);
    }

    /// <summary>
    /// Linear interpolation contract.
    /// </summary>
    public interface IInterpolateLinear<TValue>
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
    public interface IInterpolateBarycentric<TValue>
    {
        /// <summary>
        /// Interpolate in between three components passing barycentric coordinates.
        /// </summary>
        TValue InterpolateBarycentric(in TValue other0, in TValue other1, Vector3F barycentric);
    }
}
