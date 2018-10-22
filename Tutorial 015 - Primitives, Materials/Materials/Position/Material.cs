using System.Drawing;

namespace RCi.Tutorials.Gfx.Materials.Position
{
    /// <inheritdoc cref="IMaterial"/>
    public class Material :
        Materials.Material,
        IMaterial
    {
        #region // storage

        /// <inheritdoc />
        public Color Color { get; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Material(Color color)
        {
            Color = color;
        }

        #endregion
    }
}
