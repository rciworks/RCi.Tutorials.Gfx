
namespace RCi.Tutorials.Gfx.Mathematics.Extensions
{
    public static class InterpolateEx
    {
        public static float InterpolateMultiply(this float left, float multiplier)
        {
            return left * multiplier;
        }

        public static float InterpolateLinear(this float left, float right, float alpha)
        {
            return left + (right - left) * alpha;
        }

        public static float InterpolateBarycentric(this float left, float other0, float other1, Vector3F barycentric)
        {
            return left * barycentric.X + other0 * barycentric.Y + other1 * barycentric.Z;
        }
    }
}
