using System;
using RCi.Tutorials.Gfx.Drivers.Gdi.Materials;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization
{
    /// <summary>
    /// Flags representing whether vertex is inside or outside (possibly multiple) clipping planes.
    /// </summary>
    [Flags]
    public enum ClippingPlane
    {
        Inside = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        Bottom = 1 << 2,
        Top = 1 << 3,
        Near = 1 << 4,
        Far = 1 << 5,
    }

    /// <summary>
    /// Clipping options.
    /// </summary>
    public static class Clipping
    {
        public static bool CLIP_NEAR_PLANE_AT_ZERO { get; set; } = true;
    }

    /// <summary>
    /// Clipping for <see cref="TVertex"/> primitives.
    /// </summary>
    public static class Clipping<TVertex>
        where TVertex : IPsIn<TVertex>
    {
        /// <summary>
        /// Is vertex outside given plane?
        /// </summary>
        public static bool IsOutside(ClippingPlane plane, in TVertex vertex)
        {
            switch (plane)
            {
                case ClippingPlane.Inside:
                    return false;

                case ClippingPlane.Left:
                    return vertex.Position.X < -vertex.Position.W;

                case ClippingPlane.Right:
                    return vertex.Position.X > vertex.Position.W;

                case ClippingPlane.Bottom:
                    return vertex.Position.Y < -vertex.Position.W;

                case ClippingPlane.Top:
                    return vertex.Position.Y > vertex.Position.W;

                case ClippingPlane.Far:
                    return vertex.Position.Z > vertex.Position.W;

                case ClippingPlane.Near:
                    if (Clipping.CLIP_NEAR_PLANE_AT_ZERO)
                    {
                        return vertex.Position.Z < 0;
                    }
                    else
                    {
                        return vertex.Position.Z < -vertex.Position.W;
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(plane), plane, default);
            }
        }
    }
}
