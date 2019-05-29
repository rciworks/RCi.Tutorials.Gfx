using System;
using System.Runtime.InteropServices;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.PositionTexture
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct VsIn
    {
        public Vector3F Position { get; }
        public Vector2F TextureCoordinate { get; }

        public VsIn(Vector3F position, Vector2F textureCoordinate)
        {
            Position = position;
            TextureCoordinate = textureCoordinate;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct PsIn :
        IPsIn<PsIn>
    {
        public Vector4F Position { get; }
        public Vector2F TextureCoordinate { get; }

        public PsIn(Vector4F position, Vector2F textureCoordinate)
        {
            Position = position;
            TextureCoordinate = textureCoordinate;
        }

        #region // interpolation

        public PsIn InterpolateMultiply(float multiplier)
        {
            return new PsIn
            (
                Position.InterpolateMultiply(multiplier),
                TextureCoordinate.InterpolateMultiply(multiplier)
            );
        }

        public PsIn InterpolateLinear(in PsIn other, float alpha)
        {
            return new PsIn
            (
                Position.InterpolateLinear(other.Position, alpha),
                TextureCoordinate.InterpolateLinear(other.TextureCoordinate, alpha)
            );
        }

        public PsIn InterpolateBarycentric(in PsIn other0, in PsIn other1, Vector3F barycentric)
        {
            return new PsIn
            (
                Position.InterpolateBarycentric(other0.Position, other1.Position, barycentric),
                TextureCoordinate.InterpolateBarycentric(other0.TextureCoordinate, other1.TextureCoordinate, barycentric)
            );
        }

        #endregion

        public PsIn ReplacePosition(Vector4F position)
        {
            return new PsIn
            (
                position,
                TextureCoordinate
            );
        }
    }
}
