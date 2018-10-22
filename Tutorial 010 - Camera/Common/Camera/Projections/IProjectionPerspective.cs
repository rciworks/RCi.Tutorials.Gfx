namespace RCi.Tutorials.Gfx.Common.Camera.Projections
{
    /// <summary>
    /// Perspective projection.
    /// </summary>
    public interface IProjectionPerspective :
        IProjection
    {
        /// <summary>
        /// Field of view in the y direction, in radians.
        /// </summary>
        double FieldOfViewY { get; }

        /// <summary>
        /// Aspect ratio, defined as view space width divided by height.
        /// </summary>
        double AspectRatio { get; }
    }
}
