using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Defines how <see cref="IPrimitive"/> behaves in surrounding environment.
    /// </summary>
    public struct PrimitiveBehaviour
    {
        #region // storage

        /// <inheritdoc cref="Space"/>
        public Space Space { get; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PrimitiveBehaviour(Space space)
        {
            Space = space;
        }

        #endregion

        #region // factory

        /// <summary>
        /// Get default primitive behaviour.
        /// </summary>
        public static PrimitiveBehaviour Default = new PrimitiveBehaviour(Space.World);

        #endregion
    }
}
