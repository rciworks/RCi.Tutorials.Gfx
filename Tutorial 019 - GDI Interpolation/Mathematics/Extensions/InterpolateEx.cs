using System.Runtime.CompilerServices;

namespace RCi.Tutorials.Gfx.Mathematics.Extensions
{
    public static class InterpolateEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InterpolateLinear(this float left, float right, float alpha)
        {
            return left + (right - left) * alpha;
        }
    }
}
