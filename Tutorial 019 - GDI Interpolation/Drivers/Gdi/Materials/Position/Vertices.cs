using System;
using System.Runtime.InteropServices;
using RCi.Tutorials.Gfx.Mathematics;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Materials.Position
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct VsIn
    {
        public Vector3F Position { get; }

        public VsIn(Vector3F position)
        {
            Position = position;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct PsIn :
        IPsIn<PsIn>
    {
        public Vector4F Position { get; }

        public PsIn(Vector4F position)
        {
            Position = position;
        }

        #region // interpolation

        public PsIn InterpolateLinear(in PsIn other, float alpha)
        {
            return new PsIn
            (
                Position.InterpolateLinear(other.Position, alpha)
            );
        }

        #endregion

        public PsIn ReplacePosition(Vector4F position)
        {
            return new PsIn
            (
                position
            );
        }
    }
}
