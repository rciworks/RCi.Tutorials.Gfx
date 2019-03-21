using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RCi.Tutorials.Gfx.Materials.Position
{
    /// <inheritdoc cref="IPrimitive"/>
    public class Primitive :
        Primitive<IMaterial, IVertex>,
        IPrimitive
    {
        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected Primitive(PrimitiveBehaviour primitiveBehaviour, PrimitiveTopology primitiveTopology, IReadOnlyList<IVertex> vertices, Color color) :
            base(primitiveBehaviour, new Material(color), primitiveTopology, vertices)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Primitive(PrimitiveBehaviour primitiveBehaviour, PrimitiveTopology primitiveTopology, IEnumerable<IVertex> vertices, Color color) :
            this(primitiveBehaviour, primitiveTopology, vertices as IReadOnlyList<IVertex> ?? vertices.ToArray(), color)
        {
        }

        #endregion
    }
}
