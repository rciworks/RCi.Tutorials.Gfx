namespace RCi.Tutorials.Gfx.Inputs
{
    /// <inheritdoc />
    public class InputForms :
        Input
    {
        #region // storage

        /// <summary>
        /// Actual control which provides data.
        /// </summary>
        private System.Windows.Forms.Control Control { get; set; }

        /// <inheritdoc />
        public override System.Drawing.Size Size => Control.Size;

        /// <inheritdoc />
        public override event SizeEventHandler SizeChanged;

        /// <inheritdoc />
        public override event MouseEventHandler MouseMove;

        /// <inheritdoc />
        public override event MouseEventHandler MouseDown;

        /// <inheritdoc />
        public override event MouseEventHandler MouseUp;

        /// <inheritdoc />
        public override event MouseEventHandler MouseWheel;

        /// <inheritdoc />
        public override event KeyEventHandler KeyDown;

        /// <inheritdoc />
        public override event KeyEventHandler KeyUp;

        #endregion

        #region // ctor

        /// <inheritdoc />
        public InputForms(System.Windows.Forms.Control control)
        {
            // store
            Control = control;

            // hook
            Control.SizeChanged += ControlOnSizeChanged;
            Control.MouseMove += ControlOnMouseMove;
            Control.MouseDown += ControlOnMouseDown;
            Control.MouseUp += ControlOnMouseUp;
            Control.MouseWheel += ControlOnMouseWheel;
            Control.KeyDown += ControlOnKeyDown;
            Control.KeyUp += ControlOnKeyUp;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            // unhook
            Control.SizeChanged -= ControlOnSizeChanged;
            Control.MouseMove -= ControlOnMouseMove;
            Control.MouseDown -= ControlOnMouseDown;
            Control.MouseUp -= ControlOnMouseUp;
            Control.MouseWheel -= ControlOnMouseWheel;
            Control.KeyDown -= ControlOnKeyDown;
            Control.KeyUp -= ControlOnKeyUp;

            // clear storage
            Control = default;

            base.Dispose();
        }

        #endregion

        #region // handlers

        /// <inheritdoc cref="SizeChanged" />
        private void ControlOnSizeChanged(object sender, System.EventArgs args) => SizeChanged?.Invoke(sender, new SizeEventArgs(Control.Size));

        /// <inheritdoc cref="MouseMove" />
        private void ControlOnMouseMove(object sender, System.Windows.Forms.MouseEventArgs args) => MouseMove?.Invoke(sender, new MouseEventArgs(args));

        /// <inheritdoc cref="MouseDown" />
        private void ControlOnMouseDown(object sender, System.Windows.Forms.MouseEventArgs args) => MouseDown?.Invoke(sender, new MouseEventArgs(args));

        /// <inheritdoc cref="MouseUp" />
        private void ControlOnMouseUp(object sender, System.Windows.Forms.MouseEventArgs args) => MouseUp?.Invoke(sender, new MouseEventArgs(args));

        /// <inheritdoc cref="MouseWheel" />
        private void ControlOnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs args) => MouseWheel?.Invoke(sender, new MouseEventArgs(args));

        /// <inheritdoc cref="KeyDown" />
        private void ControlOnKeyDown(object sender, System.Windows.Forms.KeyEventArgs args) => KeyDown?.Invoke(sender, new KeyEventArgs(args));

        /// <inheritdoc cref="KeyUp" />
        private void ControlOnKeyUp(object sender, System.Windows.Forms.KeyEventArgs args) => KeyUp?.Invoke(sender, new KeyEventArgs(args));

        #endregion
    }
}
