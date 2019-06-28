using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Represents graphical model. Carries everything needed for rendering.
    /// </summary>
    public interface IModel
    {
        /// <inheritdoc cref="PrimitiveTopology"/>
        PrimitiveTopology PrimitiveTopology { get; set; }

        /// <summary>
        /// Position buffer.
        /// </summary>
        Vector3F[] Positions { get; set; }

        /// <summary>
        /// Color buffer.
        /// </summary>
        int[] Colors { get; set; }

        /// <summary>
        /// Texture coordinate buffer.
        /// </summary>
        Vector2F[] TextureCoordinates { get; set; }

        /// <inheritdoc cref="ITextureResource"/>
        ITextureResource[] TextureResources { get; set; }
    }
}
