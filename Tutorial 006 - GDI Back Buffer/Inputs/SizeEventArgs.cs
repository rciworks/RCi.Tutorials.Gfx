using System;

namespace RCi.Tutorials.Gfx.Inputs
{
    /// <inheritdoc cref="ISizeEventArgs" />
    public class SizeEventArgs :
        EventArgs,
        ISizeEventArgs
    {
        #region // storage

        /// <inheritdoc />
        public System.Drawing.Size NewSize { get; set; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public SizeEventArgs(System.Drawing.Size newSize)
        {
            NewSize = newSize;
        }

        #endregion
    }
}
