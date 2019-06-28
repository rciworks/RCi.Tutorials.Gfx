using System.Collections.Generic;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <inheritdoc />
    public class Visual :
        IVisual
    {
        #region // static

        /// <summary>
        /// Default <see cref="IModel"/>s.
        /// </summary>
        public static IModel[] ModelsDefault { get; } = new IModel[0];

        #endregion

        #region // storage

        /// <inheritdoc />
        public IReadOnlyList<IModel> Models { get; }

        /// <inheritdoc />
        public IMaterial Material { get; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public Visual(IReadOnlyList<IModel> models, IMaterial material)
        {
            Models = models;
            Material = material;
        }

        /// <inheritdoc />
        public Visual(IModel model, IMaterial material) :
            this(new[] { model }, material)
        {
        }

        /// <inheritdoc />
        public Visual(IReadOnlyList<IModel> models) :
            this(models, Materials.Material.Default)
        {
        }

        /// <inheritdoc />
        public Visual(IModel model) :
            this(model, Materials.Material.Default)
        {
        }

        /// <inheritdoc />
        public Visual() :
            this(ModelsDefault, Materials.Material.Default)
        {
        }

        #endregion
    }
}
