using System.Collections.Generic;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Visual which represents <see cref="IModel"/>s shaded with certain <see cref="IMaterial"/>.
    /// </summary>
    public interface IVisual
    {
        /// <summary>
        /// <see cref="IModel"/>s to be shaded.
        /// </summary>
        IReadOnlyList<IModel> Models { get; }

        /// <summary>
        /// <see cref="IMaterial"/> to be used for shading.
        /// </summary>
        IMaterial Material { get; }
    }
}
