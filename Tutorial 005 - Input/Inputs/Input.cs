using System;

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
        protected Input()
        {
            // TODO: debug
            Test.Subscribe(this);
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            // TODO: debug
            Test.Unsubscribe(this);
        }

        #endregion

        #region // test

        /// <summary>
        /// TODO: debug class to test input.
        /// </summary>
        private static class Test
        {
            public static void Subscribe(IInput input)
            {
                input.SizeChanged += InputOnSizeChanged;
                input.MouseMove += InputOnMouseMove;
                input.MouseDown += InputOnMouseDown;
                input.MouseUp += InputOnMouseUp;
                input.MouseWheel += InputOnMouseWheel;
                input.KeyDown += InputOnKeyDown;
                input.KeyUp += InputOnKeyUp;
            }

            public static void Unsubscribe(IInput input)
            {
                input.SizeChanged -= InputOnSizeChanged;
                input.MouseMove -= InputOnMouseMove;
                input.MouseDown -= InputOnMouseDown;
                input.MouseUp -= InputOnMouseUp;
                input.MouseWheel -= InputOnMouseWheel;
                input.KeyDown -= InputOnKeyDown;
                input.KeyUp -= InputOnKeyUp;
            }

            private static void InputOnSizeChanged(object sender, ISizeEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.SizeChanged)} {args.NewSize}");
            }

            private static void InputOnMouseMove(object sender, IMouseEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.MouseMove)} {args.Position}");
            }

            private static void InputOnMouseDown(object sender, IMouseEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.MouseDown)} {args.Position} {args.Buttons}");
            }

            private static void InputOnMouseUp(object sender, IMouseEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.MouseUp)} {args.Position} {args.Buttons}");
            }

            private static void InputOnMouseWheel(object sender, IMouseEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.MouseWheel)} {args.Position} {args.WheelDelta}");
            }

            private static void InputOnKeyDown(object sender, IKeyEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.KeyDown)} {args.Modifiers} {args.Key}");
            }

            private static void InputOnKeyUp(object sender, IKeyEventArgs args)
            {
                Console.WriteLine($"{nameof(IInput.KeyUp)} {args.Modifiers} {args.Key}");
            }
        }

        #endregion
    }
}
