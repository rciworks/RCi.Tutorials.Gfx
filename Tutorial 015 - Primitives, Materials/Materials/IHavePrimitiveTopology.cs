namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Has <see cref="PrimitiveTopology"/>.
    /// </summary>
    public interface IHavePrimitiveTopology
    {
        /// <inheritdoc cref="PrimitiveTopology"/>
        PrimitiveTopology PrimitiveTopology { get; }
    }
}
