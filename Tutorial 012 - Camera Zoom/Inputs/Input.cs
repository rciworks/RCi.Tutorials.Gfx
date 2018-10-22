namespace RCi.Tutorials.Gfx.Inputs
{
    /// <inheritdoc />
    public abstract class Input :
        IInput
    {
        #region // storage

        /// <inheritdoc />
        public abstract System.Drawing.Size Size { get; }

        /// <inheritdoc />
        public abstract event SizeEventHandler SizeChanged;

        /// <inheritdoc />
        public abstract event MouseEventHandler MouseMove;

        /// <inheritdoc />
        public abstract event MouseEventHandler MouseDown;

        /// <inheritdoc />
        public abstract event MouseEventHandler MouseUp;

        /// <inheritdoc />
        public abstract event MouseEventHandler MouseWheel;

        /// <inheritdoc />
        public abstract event KeyEventHandler KeyDown;

        /// <inheritdoc />
        public abstract event KeyEventHandler KeyUp;

        #endregion

        #region // ctor

        /// <inheritdoc />
        public abstract void Dispose();

        #endregion
    }
}
