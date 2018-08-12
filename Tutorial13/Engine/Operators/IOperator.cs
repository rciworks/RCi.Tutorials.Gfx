using System;

namespace RCi.Tutorials.Gfx.Engine.Operators
{
    /// <summary>
    /// Isolated component providing functionality mostly based on events
    /// provided by <see cref="Render.IRenderHost"/> or components it owns.
    /// </summary>
    public interface IOperator :
        IDisposable
    {
    }
}
