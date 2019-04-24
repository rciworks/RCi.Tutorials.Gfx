namespace RCi.Tutorials.Gfx.Materials
{
    /// <summary>
    /// Define how vertices are interpreted and rendered by the pipeline.
    /// </summary>
    public enum PrimitiveTopology
    {
        /// <summary>
        /// Undefined primitive topology.
        /// </summary>
        Undefined,

        /// <summary>
        /// A point list is a collection of vertices that are rendered as isolated points.
        /// </summary>
        PointList,

        /// <summary>
        /// A line list is a list of isolated, straight line segments.
        /// Note that the number of vertices in a line list must be an even number greater than or equal to two.
        /// </summary>
        LineList,

        /// <summary>
        /// A line strip is a primitive that is composed of connected line segments.
        /// </summary>
        LineStrip,

        /// <summary>
        /// A triangle list is a list of isolated triangles. They might or might not be near each other.
        /// A triangle list must have at least three vertices and the total number of vertices must be divisible by three.
        /// </summary>
        TriangleList,

        /// <summary>
        /// A triangle strip is a series of connected triangles. Because the triangles are connected,
        /// the application does not need to repeatedly specify all three vertices for each triangle.
        /// For example, you need only seven vertices to define the following triangle strip.
        /// </summary>
        TriangleStrip,
    }
}
