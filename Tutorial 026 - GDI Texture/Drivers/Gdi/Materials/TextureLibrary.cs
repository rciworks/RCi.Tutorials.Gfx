using RCi.Tutorials.Gfx.Materials;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials
{
    /// <inheritdoc />
    public class TextureLibrary :
        TextureLibrary<ITextureResource, Texture>
    {
        /// <inheritdoc />
        protected override Texture CreateTexture(ITextureResource textureResource)
        {
            return textureResource.Source is null
                ? new Texture(textureResource.Size)     // empty
                : new Texture(textureResource.Source);  // initial source
        }

        /// <inheritdoc />
        protected override void DeleteTexture(Texture texture)
        {
            texture.Dispose();
        }
    }
}
