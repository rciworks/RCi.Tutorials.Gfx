namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization
{
    /// <summary>
    /// Provides construction for vertex shader input type vertices from given raw data.
    /// </summary>
    public interface IBufferBinding<out TVsIn>
        where TVsIn : unmanaged
    {
        /// <summary>
        /// Construct vertex shader input at given index.
        /// </summary>
        TVsIn GetVsIn(uint index);
    }
}
