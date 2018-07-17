using MathNet.Spatial.Euclidean;

namespace RCi.Tutorials.Gfx.Inputs
{
    /// <summary>
    /// Mouse event arguments.
    /// </summary>
    public interface IMouseEventArgs
    {
        /// <summary>
        /// Position of the mouse during mouse event.
        /// </summary>
        Point2D Position { get; }

        /// <inheritdoc cref="MouseButtons"/>
        MouseButtons Buttons { get; }

        /// <summary>
        /// Signed count of the number of detents the mouse wheel has rotated.
        /// </summary>
        int WheelDelta { get; }

        /// <summary>
        /// Number of times the mouse button was pressed and released.
        /// </summary>
        int ClickCount { get; }
    }
}
