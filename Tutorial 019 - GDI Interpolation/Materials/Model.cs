using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <inheritdoc />
    public class Model :
        IModel
    {
        /// <inheritdoc />
        public ShaderType ShaderType { get; set; }

        /// <inheritdoc />
        public Space Space { get; set; }

        /// <inheritdoc />
        public PrimitiveTopology PrimitiveTopology { get; set; }

        /// <inheritdoc />
        public Vector3F[] Positions { get; set; }

        /// <inheritdoc />
        public int[] Colors { get; set; }

        /// <inheritdoc />
        public int Color { get; set; }
    }
}
