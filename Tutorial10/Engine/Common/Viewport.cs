namespace RCi.Tutorials.Gfx.Engine.Common
{
    public struct Viewport
    {
        #region // storage

        /// <summary>
        /// X pixel coordinate of the upper-left corner of the viewport on the render target surface.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Y pixel coordinate of the upper-left corner of the viewport on the render target surface.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Width dimension of the viewport on the render target surface, in pixels.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Height dimension of the viewport on the render target surface, in pixels.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Minimum value of the clip volume.
        /// </summary>
        public double MinZ { get; }

        /// <summary>
        /// Maximum value of the clip volume.
        /// </summary>
        public double MaxZ { get; }

        #endregion

        #region // queries

        /// <summary>
        /// Pixel coordinate of the upper-left corner of the viewport on the render target surface.
        /// </summary>
        public System.Drawing.Point Location => new System.Drawing.Point(X, Y);

        /// <summary>
        /// Size of the viewport on the render target surface, in pixels.
        /// </summary>
        public System.Drawing.Size Size => new System.Drawing.Size(Width, Height);

        /// <summary>
        /// Width to height (X:Y) ratio of the viewport.
        /// </summary>
        public double AspectRatio => (double)Width / Height;

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Viewport(int x, int y, int width, int height, double minZ, double maxZ)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            MinZ = minZ;
            MaxZ = maxZ;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Viewport(System.Drawing.Point location, System.Drawing.Size size, double minZ, double maxZ) :
            this(location.X, location.Y, size.Width, size.Height, minZ, maxZ)
        {
        }

        #endregion
    }
}
