using System;
using System.Collections.Generic;
using System.Drawing;

namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Texture resource.
    /// </summary>
    public interface ITextureResource :
        IEqualityComparer<ITextureResource>,
        IDisposable
    {
        /// <summary>
        /// Texture id (unique, auto-generated).
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Texture name (unique).
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Texture size in pixels.
        /// </summary>
        Size Size { get; }

        /// <summary>
        /// Texture source (optional).
        /// </summary>
        Bitmap Source { get; }
    }
}