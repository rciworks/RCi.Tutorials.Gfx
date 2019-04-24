using System;

namespace RCi.Tutorials.Gfx.Inputs
{
    /// <summary>
    /// Provides properties and input events spawned by the component.
    /// </summary>
    public interface IInput :
        IDisposable
    {
        /// <summary>
        /// The size of the component.
        /// </summary>
        System.Drawing.Size Size { get; }

        /// <summary>
        /// Occurs when size of the component changes.
        /// </summary>
        event SizeEventHandler SizeChanged;

        /// <summary>
        /// Occurs when the mouse pointer is moved over the component.
        /// </summary>
        event MouseEventHandler MouseMove;

        /// <summary>
        /// Occurs when the mouse pointer is over the component and a mouse button is pressed.
        /// </summary>
        event MouseEventHandler MouseDown;

        /// <summary>
        /// Occurs when the mouse pointer is over the component and a mouse button is released.
        /// </summary>
        event MouseEventHandler MouseUp;

        /// <summary>
        /// Occurs when the mouse wheel moves while the component has focus.
        /// </summary>
        event MouseEventHandler MouseWheel;

        /// <summary>
        /// Occurs when a key is pressed down while the component has focus.
        /// </summary>
        event KeyEventHandler KeyDown;

        /// <summary>
        /// Occurs when a key is released while the component has focus.
        /// </summary>
        event KeyEventHandler KeyUp;
    }
}
