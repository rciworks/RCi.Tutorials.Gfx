using System.Collections.Generic;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <inheritdoc cref="IPrimitive"/>
    public abstract class Primitive :
        IPrimitive
    {
        #region // storage

        /// <inheritdoc />
        public PrimitiveBehaviour PrimitiveBehaviour { get; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected Primitive(PrimitiveBehaviour primitiveBehaviour)
        {
            PrimitiveBehaviour = primitiveBehaviour;
        }

        #endregion
    }

    /// <inheritdoc cref="IPrimitive{TMaterial}"/>
    public abstract class Primitive<TMaterial> :
        Primitive,
        IPrimitive<TMaterial>
        where TMaterial : IMaterial
    {
        #region // storage

        /// <inheritdoc />
        public TMaterial Material { get; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected Primitive(PrimitiveBehaviour primitiveBehaviour, TMaterial material) :
            base(primitiveBehaviour)
        {
            Material = material;
        }

        #endregion
    }

    /// <inheritdoc cref="IPrimitive{TMaterial, TVertex}"/>
    public abstract class Primitive<TMaterial, TVertex> :
        Primitive<TMaterial>,
        IPrimitive<TMaterial, TVertex>
        where TMaterial : IMaterial
        where TVertex : IVertex
    {
        #region // storage

        /// <inheritdoc />
        public PrimitiveTopology PrimitiveTopology { get; }

        /// <inheritdoc />
        public IReadOnlyList<TVertex> Vertices { get; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected Primitive(PrimitiveBehaviour primitiveBehaviour, TMaterial material, PrimitiveTopology primitiveTopology, IReadOnlyList<TVertex> vertices) :
            base(primitiveBehaviour, material)
        {
            PrimitiveTopology = primitiveTopology;
            Vertices = vertices;
        }

        #endregion
    }
}
