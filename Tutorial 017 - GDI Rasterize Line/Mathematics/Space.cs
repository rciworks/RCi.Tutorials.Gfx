using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RCi.Tutorials.Gfx.Mathematics
{
    /// <summary>
    /// Graphics context space.
    /// </summary>
    public enum Space
    {
        /// <summary>
        /// World space. Operating in world units.
        /// </summary>
        World,

        /// <summary>
        /// View space or NDC (Normalized Device Coordinates). Operating in NDC units (usually [-1..1]).
        /// </summary>
        View,

        /// <summary>
        /// Screen space. Operating in pixel units.
        /// </summary>
        Screen,
    }

    public static class SpaceEx
    {
        /// <summary>
        /// All available spaces.
        /// </summary>
        public static Space[] SpaceValues { get; } = (Space[])Enum.GetValues(typeof(Space));

        /// <summary>
        /// Total spaces count.
        /// </summary>
        public static int SpaceCount { get; } = SpaceValues.Length;

        /// <summary>
        /// All available spaces as integers.
        /// </summary>
        public static int[] SpaceIds { get; } = Enumerable.Range(0, SpaceCount).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToSpaceId(this Space space)
        {
            return (int)space;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Space ToSpace(this int spaceId)
        {
            return (Space)spaceId;
        }
    }
}
