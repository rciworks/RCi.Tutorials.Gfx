using System;

namespace RCi.Tutorials.Gfx.Inputs
{
    /// <summary>
    /// Flags representing mouse buttons.
    /// </summary>
    [Flags]
    public enum MouseButtons
    {
        None =
            0b_0000_0000,

        Left =
            0b_0000_0001,

        Middle =
            0b_0000_0010,

        Right =
            0b_0000_0100,

        XButton1 =
            0b_0000_1000,

        XButton2 =
            0b_0001_0000,
    }
}
