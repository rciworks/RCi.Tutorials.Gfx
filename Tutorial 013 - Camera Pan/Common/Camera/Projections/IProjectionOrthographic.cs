namespace RCi.Tutorials.Gfx.Common.Camera.Projections
{
    /// <summary>
    /// Orthographic projection.
    /// </summary>
    public interface IProjectionOrthographic :
        IProjection
    {
        /// <summary>
        /// Width of the view volume.
        /// </summary>
        double FieldWidth { get; }

        /// <summary>
        /// Height of the view volume.
        /// </summary>
        double FieldHeight { get; }
    }
}
