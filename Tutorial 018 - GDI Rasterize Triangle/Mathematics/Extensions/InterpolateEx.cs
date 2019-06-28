
namespace RCi.Tutorials.Gfx.Mathematics.Extensions
{
    public static class InterpolateEx
    {
        public static float InterpolateLinear(this float left, float right, float alpha)
        {
            return left + (right - left) * alpha;
        }
    }
}
