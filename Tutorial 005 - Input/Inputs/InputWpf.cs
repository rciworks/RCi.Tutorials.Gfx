using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Inputs
{
    /// <inheritdoc />
    public class InputWpf :
        Input
    {
        #region // storage

        /// <summary>
        /// Actual control which provides data.
        /// </summary>
        private System.Windows.FrameworkElement Control { get; set; }

        /// <inheritdoc />
        public override System.Drawing.Size Size => new System.Drawing.Size((int)Control.ActualWidth, (int)Control.ActualHeight);

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
        public InputWpf(System.Windows.FrameworkElement control)
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
        private void ControlOnSizeChanged(object sender, System.Windows.SizeChangedEventArgs args) => SizeChanged?.Invoke(sender, new SizeEventArgs(Size));

        /// <inheritdoc cref="MouseMove" />
        private void ControlOnMouseMove(object sender, System.Windows.Input.MouseEventArgs args) => MouseMove?.Invoke(sender, new MouseEventArgs(args, GetPosition(args), 0));

        /// <inheritdoc cref="MouseDown" />
        private void ControlOnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs args) => MouseDown?.Invoke(sender, new MouseEventArgs(args, GetPosition(args), 0));

        /// <inheritdoc cref="MouseUp" />
        private void ControlOnMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs args) => MouseUp?.Invoke(sender, new MouseEventArgs(args, GetPosition(args), 0));

        /// <inheritdoc cref="MouseWheel" />
        private void ControlOnMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs args) => MouseWheel?.Invoke(sender, new MouseEventArgs(args, GetPosition(args)));

        /// <inheritdoc cref="KeyDown" />
        private void ControlOnKeyDown(object sender, System.Windows.Input.KeyEventArgs args) => KeyDown?.Invoke(sender, new KeyEventArgs(args));

        /// <inheritdoc cref="KeyUp" />
        private void ControlOnKeyUp(object sender, System.Windows.Input.KeyEventArgs args) => KeyUp?.Invoke(sender, new KeyEventArgs(args));

        #endregion

        #region // routines

        private Point2D GetPosition(System.Windows.Input.MouseEventArgs args)
        {
            return args.GetPosition(Control).ToPoint2D();
        }

        #endregion
    }
}
