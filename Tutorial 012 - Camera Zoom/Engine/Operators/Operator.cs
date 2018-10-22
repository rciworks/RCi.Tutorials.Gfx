using RCi.Tutorials.Gfx.Engine.Render;
using RCi.Tutorials.Gfx.Inputs;

namespace RCi.Tutorials.Gfx.Engine.Operators
{
    /// <inheritdoc cref="IOperator"/>
    public abstract class Operator :
        IOperator
    {
        #region // storage

        /// <summary>
        /// Owner.
        /// </summary>
        protected IRenderHost RenderHost { get; private set; }

        /// <summary>
        /// Forwarded <see cref="IInput"/>.
        /// </summary>
        protected IInput Input => RenderHost.HostInput;

        #endregion

        #region // ctor

        /// <inheritdoc />
        protected Operator(IRenderHost renderHost)
        {
            RenderHost = renderHost;

            Input.SizeChanged += InputOnSizeChanged;
            Input.KeyDown += InputOnKeyDown;
            Input.KeyUp += InputOnKeyUp;
            Input.MouseMove += InputOnMouseMove;
            Input.MouseDown += InputOnMouseDown;
            Input.MouseUp += InputOnMouseUp;
            Input.MouseWheel += InputOnMouseWheel;
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            Input.SizeChanged -= InputOnSizeChanged;
            Input.KeyDown -= InputOnKeyDown;
            Input.KeyUp -= InputOnKeyUp;
            Input.MouseMove -= InputOnMouseMove;
            Input.MouseDown -= InputOnMouseDown;
            Input.MouseUp -= InputOnMouseUp;
            Input.MouseWheel -= InputOnMouseWheel;

            RenderHost = default;
        }

        #endregion

        #region // sensors

        protected virtual void InputOnSizeChanged(object sender, ISizeEventArgs args)
        {
        }

        protected virtual void InputOnKeyDown(object sender, IKeyEventArgs args)
        {
        }

        protected virtual void InputOnKeyUp(object sender, IKeyEventArgs args)
        {
        }

        protected virtual void InputOnMouseMove(object sender, IMouseEventArgs args)
        {
        }

        protected virtual void InputOnMouseDown(object sender, IMouseEventArgs args)
        {
        }

        protected virtual void InputOnMouseUp(object sender, IMouseEventArgs args)
        {
        }

        protected virtual void InputOnMouseWheel(object sender, IMouseEventArgs args)
        {
        }

        #endregion
    }
}
