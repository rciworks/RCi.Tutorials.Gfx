namespace RCi.Tutorials.Gfx.Common.Camera.Projections
{
    /// <summary>
    /// Projection combining two other <see cref="IProjection"/>s with certain ratio.
    /// </summary>
    public interface IProjectionCombined :
        IProjection
    {
        /// <summary>
        /// First projection.
        /// </summary>
        IProjection Projection0 { get; }

        /// <summary>
        /// Second projection.
        /// </summary>
        IProjection Projection1 { get; }

        /// <summary>
        /// Weight of <see cref="Projection0"/>
        /// </summary>
        double Weight0 { get; }

        /// <summary>
        /// Weight of <see cref="Projection1"/>
        /// </summary>
        double Weight1 { get; }
    }
}
