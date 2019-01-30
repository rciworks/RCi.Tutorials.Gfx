using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Represents graphical model. Carries everything needed for rendering.
    /// </summary>
    public interface IModel
    {
        /// <inheritdoc cref="ShaderType"/>
        ShaderType ShaderType { get; set; }

        /// <inheritdoc cref="Space"/>
        Space Space { get; set; }

        /// <inheritdoc cref="PrimitiveTopology"/>
        PrimitiveTopology PrimitiveTopology { get; set; }

        /// <summary>
        /// Position buffer.
        /// </summary>
        Vector3F[] Positions { get; set; }

        /// <summary>
        /// Color for whole model.
        /// </summary>
        int Color { get; set; }
    }
}
