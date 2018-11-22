using System;
using System.Runtime.CompilerServices;
using MathNet.Spatial.Euclidean;

namespace RCi.Tutorials.Gfx.Mathematics.Extensions
{
    public static class QuaternionEx
    {
        public static UnitVector3D Axis(this in Quaternion value)
        {
            var a = value.ImagX * value.ImagX + value.ImagY * value.ImagY + value.ImagZ * value.ImagZ;
            if (Math.Abs(a) < 10E-15)
            {
                return UnitVector3D.XAxis;
            }
            var num = 1.0 / Math.Sqrt(a);
            return new UnitVector3D(value.ImagX * num, value.ImagY * num, value.ImagZ * num);
        }

        public static Quaternion Rotation(double yaw, double pitch, double roll)
        {
            var halfYaw = yaw * 0.5;
            var halfPitch = pitch * 0.5;
            var halfRoll = roll * 0.5;
            var sinHalfYaw = Math.Sin(halfYaw);
            var cosHalfYaw = Math.Cos(halfYaw);
            var sinHalfPitch = Math.Sin(halfPitch);
            var cosHalfPitch = Math.Cos(halfPitch);
            var sinHalfRoll = Math.Sin(halfRoll);
            var cosHalfRoll = Math.Cos(halfRoll);

            var x = cosHalfYaw * sinHalfPitch * cosHalfRoll + sinHalfYaw * cosHalfPitch * sinHalfRoll;
            var y = sinHalfYaw * cosHalfPitch * cosHalfRoll - cosHalfYaw * sinHalfPitch * sinHalfRoll;
            var z = cosHalfYaw * cosHalfPitch * sinHalfRoll - sinHalfYaw * sinHalfPitch * cosHalfRoll;
            var w = cosHalfYaw * cosHalfPitch * cosHalfRoll + sinHalfYaw * sinHalfPitch * sinHalfRoll;

            return new Quaternion(w, x, y, z);
        }

        public static Quaternion FromVectorToVector(in UnitVector3D fromVector, in UnitVector3D toVector)
        {
            if (fromVector == toVector)
            {
                return Quaternion.One;
            }
            var cross = fromVector.CrossProduct(toVector);
            var real = Math.Sqrt(Math.Pow(fromVector.Length, 2) * Math.Pow(toVector.Length, 2)) + fromVector.DotProduct(toVector);
            return new Quaternion(real, cross.X, cross.Y, cross.Z).Normalized;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion AroundAxis(in UnitVector3D axis, double angle)
        {
            return MatrixEx.Rotate(axis, angle).ToQuaternion();
        }
    }
}
