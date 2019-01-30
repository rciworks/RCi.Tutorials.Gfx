using System;
using System.Runtime.InteropServices;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.PositionColor
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct VsIn
    {
        public Vector3F Position { get; }
        public Vector4F Color { get; }

        public VsIn(Vector3F position, Vector4F color)
        {
            Position = position;
            Color = color;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct PsIn :
        IPsIn<PsIn>
    {
        public Vector4F Position { get; }
        public Vector4F Color { get; }

        public PsIn(Vector4F position, Vector4F color)
        {
            Position = position;
            Color = color;
        }

        #region // interpolation

        public PsIn InterpolateMultiply(float multiplier)
        {
            return new PsIn
            (
                Position.InterpolateMultiply(multiplier),
                Color.InterpolateMultiply(multiplier)
            );
        }

        public PsIn InterpolateLinear(in PsIn other, float alpha)
        {
            return new PsIn
            (
                Position.InterpolateLinear(other.Position, alpha),
                Color.InterpolateLinear(other.Color, alpha)
            );
        }

        public PsIn InterpolateBarycentric(in PsIn other0, in PsIn other1, Vector3F barycentric)
        {
            return new PsIn
            (
                Position.InterpolateBarycentric(other0.Position, other1.Position, barycentric),
                Color.InterpolateBarycentric(other0.Color, other1.Color, barycentric)
            );
        }

        #endregion

        public PsIn ReplacePosition(Vector4F position)
        {
            return new PsIn
            (
                position,
                Color
            );
        }
    }
}
