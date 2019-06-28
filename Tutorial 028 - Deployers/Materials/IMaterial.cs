using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Material to use for model rendering.
    /// </summary>
    public interface IMaterial
    {
        /// <inheritdoc cref="Space"/>
        Space Space { get; set; }
    }
}
