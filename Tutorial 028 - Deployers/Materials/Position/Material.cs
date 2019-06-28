using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Materials.Position
{
    /// <inheritdoc />
    public class Material :
        Materials.Material
    {
        /// <summary>
        /// Color for model.
        /// </summary>
        public int Color { get; }

        /// <inheritdoc />
        public Material(Space space, int color) :
            base(space)
        {
            Color = color;
        }
    }
}
