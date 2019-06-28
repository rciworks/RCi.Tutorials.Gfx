using System;
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

        public static Quaternion ToQuaternion(this in Matrix4D matrix)
        {
            double x, y, z, w;

            var num1 = matrix.M00 + matrix.M11 + matrix.M22;
            if (num1 > 0.0)
            {
                var num2 = Math.Sqrt(num1 + 1);
                w = num2 * 0.5;
                var num3 = 0.5 / num2;
                x = (matrix.M12 - matrix.M21) * num3;
                y = (matrix.M20 - matrix.M02) * num3;
                z = (matrix.M01 - matrix.M10) * num3;
            }
            else if (matrix.M00 >= matrix.M11 && matrix.M00 >= matrix.M22)
            {
                var num2 = Math.Sqrt(1.0 + matrix.M00 - matrix.M11 - matrix.M22);
                var num3 = 0.5 / num2;
                x = 0.5 * num2;
                y = (matrix.M01 + matrix.M10) * num3;
                z = (matrix.M02 + matrix.M20) * num3;
                w = (matrix.M12 - matrix.M21) * num3;
            }
            else if (matrix.M11 > matrix.M22)
            {
                var num2 = Math.Sqrt(1.0 + matrix.M11 - matrix.M00 - matrix.M22);
                var num3 = 0.5 / num2;
                x = (matrix.M10 + matrix.M01) * num3;
                y = 0.5 * num2;
                z = (matrix.M21 + matrix.M12) * num3;
                w = (matrix.M20 - matrix.M02) * num3;
            }
            else
            {
                var num2 = Math.Sqrt(1.0 + matrix.M22 - matrix.M00 - matrix.M11);
                var num3 = 0.5 / num2;
                x = (matrix.M20 + matrix.M02) * num3;
                y = (matrix.M21 + matrix.M12) * num3;
                z = 0.5 * num2;
                w = (matrix.M01 - matrix.M10) * num3;
            }

            return new Quaternion(w, x, y, z);
        }

        public static Quaternion AroundAxis(in UnitVector3D axis, double angle)
        {
            return Matrix4DEx.Rotate(axis, angle).ToQuaternion();
        }
    }
}
