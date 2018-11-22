using System.Drawing;

namespace RCi.Tutorials.Gfx.Materials.Position
{
    /// <inheritdoc cref="IPrimitive"/>
    public class Primitive :
        Primitive<IMaterial, Vertex>,
        IPrimitive
    {
        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Primitive(PrimitiveBehaviour primitiveBehaviour, PrimitiveTopology primitiveTopology, Vertex[] vertices, Color color) :
            base(primitiveBehaviour, new Material(color), primitiveTopology, vertices)
        {
        }

        #endregion
    }
}
