using System.Collections.Generic;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Has <see cref="IReadOnlyList{TVertex}"/>.
    /// </summary>
    public interface IHaveVertices<out TVertex>
    {
        /// <summary>
        /// Collection of <see cref="TVertex"/>.
        /// </summary>
        TVertex[] Vertices { get; }
    }
}
