using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <inheritdoc cref="IMaterial"/>
    public abstract class Material :
        IMaterial
    {
        /// <summary>
        /// Default <see cref="IMaterial"/>.
        /// </summary>
        public static Default.Material Default { get; } = new Default.Material(Space.View);

        /// <inheritdoc />
        public Space Space { get; set; }

        /// <inheritdoc />
        protected Material(Space space)
        {
            Space = space;
        }
    }
}
