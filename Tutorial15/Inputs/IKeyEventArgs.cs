namespace RCi.Tutorials.Gfx.Inputs
{
    /// <summary>
    /// Key event arguments.
    /// </summary>
    public interface IKeyEventArgs
    {
        /// <inheritdoc cref="System.Windows.Input.Key"/>
        Key Key { get; }

        /// <inheritdoc cref="Modifiers"/>
        Modifiers Modifiers { get; }
    }
}
