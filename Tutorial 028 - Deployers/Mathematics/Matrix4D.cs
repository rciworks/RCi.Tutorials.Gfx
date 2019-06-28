using System;
using System.Runtime.InteropServices;
using MathNet.Spatial.Euclidean;

namespace RCi.Tutorials.Gfx.Mathematics
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 16)]
    public readonly struct Matrix4D :
        IInterpolateDoubleLinear<Matrix4D>
    {
        #region // storage

        [FieldOffset(0)] public readonly double M00;
        [FieldOffset(8)] public readonly double M01;
        [FieldOffset(16)] public readonly double M02;
        [FieldOffset(24)] public readonly double M03;

        [FieldOffset(32)] public readonly double M10;
        [FieldOffset(40)] public readonly double M11;
        [FieldOffset(48)] public readonly double M12;
        [FieldOffset(56)] public readonly double M13;

        [FieldOffset(64)] public readonly double M20;
        [FieldOffset(72)] public readonly double M21;
        [FieldOffset(80)] public readonly double M22;
        [FieldOffset(88)] public readonly double M23;

        [FieldOffset(96)] public readonly double M30;
        [FieldOffset(104)] public readonly double M31;
        [FieldOffset(112)] public readonly double M32;
        [FieldOffset(120)] public readonly double M33;

        [FieldOffset(0)] public readonly Vector4D Row0;
        [FieldOffset(32)] public readonly Vector4D Row1;
        [FieldOffset(64)] public readonly Vector4D Row2;
        [FieldOffset(96)] public readonly Vector4D Row3;

        #endregion

        #region // ctor

        public Matrix4D
        (
            double m00, double m01, double m02, double m03,
            double m10, double m11, double m12, double m13,
            double m20, double m21, double m22, double m23,
            double m30, double m31, double m32, double m33
        )
            : this()
        {
            M00 = m00;
            M01 = m01;
            M02 = m02;
            M03 = m03;
            M10 = m10;
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M20 = m20;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M30 = m30;
            M31 = m31;
            M32 = m32;
            M33 = m33;
        }

        public Matrix4D(in Vector4D row0, in Vector4D row1, in Vector4D row2, in Vector4D row3)
            : this()
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }

        #endregion

        #region // static

        public static readonly Matrix4D Zero = new Matrix4D
        (
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0
        );

        public static readonly Matrix4D Identity = new Matrix4D
        (
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        );

        #endregion

        #region // operators

        public static Matrix4D operator *(in Matrix4D left, in Matrix4D right)
        {
            Multiply(left, right, out var result);
            return result;
        }

        public static Vector4D operator *(in Matrix4D left, in Vector4D right)
        {
            Multiply(left, right, out var result);
            return result;
        }

        public static Vector3D operator *(in Matrix4D left, in Vector3D right)
        {
            Multiply(left, right, out var result);
            return result;
        }

        #endregion

        #region // multiply

        public static void Multiply(in Matrix4D left, in Matrix4D right, out Matrix4D result)
        {
            // row major
            result = new Matrix4D
            (
                left.M00 * right.M00 + left.M01 * right.M10 + left.M02 * right.M20 + left.M03 * right.M30,
                left.M00 * right.M01 + left.M01 * right.M11 + left.M02 * right.M21 + left.M03 * right.M31,
                left.M00 * right.M02 + left.M01 * right.M12 + left.M02 * right.M22 + left.M03 * right.M32,
                left.M00 * right.M03 + left.M01 * right.M13 + left.M02 * right.M23 + left.M03 * right.M33,

                left.M10 * right.M00 + left.M11 * right.M10 + left.M12 * right.M20 + left.M13 * right.M30,
                left.M10 * right.M01 + left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31,
                left.M10 * right.M02 + left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32,
                left.M10 * right.M03 + left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33,

                left.M20 * right.M00 + left.M21 * right.M10 + left.M22 * right.M20 + left.M23 * right.M30,
                left.M20 * right.M01 + left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31,
                left.M20 * right.M02 + left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32,
                left.M20 * right.M03 + left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33,

                left.M30 * right.M00 + left.M31 * right.M10 + left.M32 * right.M20 + left.M33 * right.M30,
                left.M30 * right.M01 + left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31,
                left.M30 * right.M02 + left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32,
                left.M30 * right.M03 + left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33
            );

            //// column major
            //result = new Matrix4D
            //(
            //    left.M00 * right.M00 + left.M01 * right.M10 + left.M02 * right.M20 + left.M03 * right.M30,
            //    left.M00 * right.M01 + left.M01 * right.M11 + left.M02 * right.M21 + left.M03 * right.M31,
            //    left.M00 * right.M02 + left.M01 * right.M12 + left.M02 * right.M22 + left.M03 * right.M32,
            //    left.M00 * right.M03 + left.M01 * right.M13 + left.M02 * right.M23 + left.M03 * right.M33,

            //    left.M10 * right.M00 + left.M11 * right.M10 + left.M12 * right.M20 + left.M13 * right.M30,
            //    left.M10 * right.M01 + left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31,
            //    left.M10 * right.M02 + left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32,
            //    left.M10 * right.M03 + left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33,

            //    left.M20 * right.M00 + left.M21 * right.M10 + left.M22 * right.M20 + left.M23 * right.M30,
            //    left.M20 * right.M01 + left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31,
            //    left.M20 * right.M02 + left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32,
            //    left.M20 * right.M03 + left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33,

            //    left.M30 * right.M00 + left.M31 * right.M10 + left.M32 * right.M20 + left.M33 * right.M30,
            //    left.M30 * right.M01 + left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31,
            //    left.M30 * right.M02 + left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32,
            //    left.M30 * right.M03 + left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33
            //);
        }

        public static void Multiply(in Matrix4D left, in Vector4D right, out Vector4D result)
        {
            result = new Vector4D
            (
                left.M00 * right.X + left.M10 * right.Y + left.M20 * right.Z + left.M30 * right.W,
                left.M01 * right.X + left.M11 * right.Y + left.M21 * right.Z + left.M31 * right.W,
                left.M02 * right.X + left.M12 * right.Y + left.M22 * right.Z + left.M32 * right.W,
                left.M03 * right.X + left.M13 * right.Y + left.M23 * right.Z + left.M33 * right.W
            );
        }

        public static void Multiply(in Matrix4D left, in Vector3D right, out Vector3D result)
        {
            var wInv = 1 / (left.M03 * right.X + left.M13 * right.Y + left.M23 * right.Z + left.M33);
            result = new Vector3D
            (
                (left.M00 * right.X + left.M10 * right.Y + left.M20 * right.Z + left.M30) * wInv,
                (left.M01 * right.X + left.M11 * right.Y + left.M21 * right.Z + left.M31) * wInv,
                (left.M02 * right.X + left.M12 * right.Y + left.M22 * right.Z + left.M32) * wInv
            );
        }

        #endregion

        #region // interpolation

        public Matrix4D InterpolateLinear(in Matrix4D other, double alpha)
        {
            return new Matrix4D
            (
                Row0.InterpolateLinear(other.Row0, alpha),
                Row1.InterpolateLinear(other.Row1, alpha),
                Row2.InterpolateLinear(other.Row2, alpha),
                Row3.InterpolateLinear(other.Row3, alpha)
            );
        }

        #endregion

        #region // routines

        public static void Transpose(in Matrix4D value, out Matrix4D result)
        {
            result = new Matrix4D
            (
                value.M00, value.M10, value.M20, value.M30,
                value.M01, value.M11, value.M21, value.M31,
                value.M02, value.M12, value.M22, value.M32,
                value.M03, value.M13, value.M23, value.M33
            );
        }

        public Matrix4D Transpose()
        {
            Transpose(this, out var result);
            return result;
        }

        public static void Inverse(in Matrix4D value, out Matrix4D result)
        {
            var num1 = value.M20 * value.M31 - value.M21 * value.M30;
            var num2 = value.M20 * value.M32 - value.M22 * value.M30;
            var num3 = value.M23 * value.M30 - value.M20 * value.M33;
            var num4 = value.M21 * value.M32 - value.M22 * value.M31;
            var num5 = value.M23 * value.M31 - value.M21 * value.M33;
            var num6 = value.M22 * value.M33 - value.M23 * value.M32;
            var num7 = value.M11 * num6 + value.M12 * num5 + value.M13 * num4;
            var num8 = value.M10 * num6 + value.M12 * num3 + value.M13 * num2;
            var num9 = value.M10 * -num5 + value.M11 * num3 + value.M13 * num1;
            var num10 = value.M10 * num4 + value.M11 * -num2 + value.M12 * num1;
            var num11 = value.M00 * num7 - value.M01 * num8 + value.M02 * num9 - value.M03 * num10;

            if (Math.Abs(num11).Equals(0d))
            {
                result = Zero;
            }
            else
            {
                var num12 = 1f / num11;

                var num13 = value.M00 * value.M11 - value.M01 * value.M10;
                var num14 = value.M00 * value.M12 - value.M02 * value.M10;
                var num15 = value.M03 * value.M10 - value.M00 * value.M13;
                var num16 = value.M01 * value.M12 - value.M02 * value.M11;
                var num17 = value.M03 * value.M11 - value.M01 * value.M13;
                var num18 = value.M02 * value.M13 - value.M03 * value.M12;

                var num19 = value.M01 * num6 + value.M02 * num5 + value.M03 * num4;
                var num20 = value.M00 * num6 + value.M02 * num3 + value.M03 * num2;
                var num21 = value.M00 * -num5 + value.M01 * num3 + value.M03 * num1;
                var num22 = value.M00 * num4 + value.M01 * -num2 + value.M02 * num1;
                var num23 = value.M31 * num18 + value.M32 * num17 + value.M33 * num16;
                var num24 = value.M30 * num18 + value.M32 * num15 + value.M33 * num14;
                var num25 = value.M30 * -num17 + value.M31 * num15 + value.M33 * num13;
                var num26 = value.M30 * num16 + value.M31 * -num14 + value.M32 * num13;
                var num27 = value.M21 * num18 + value.M22 * num17 + value.M23 * num16;
                var num28 = value.M20 * num18 + value.M22 * num15 + value.M23 * num14;
                var num29 = value.M20 * -num17 + value.M21 * num15 + value.M23 * num13;
                var num30 = value.M20 * num16 + value.M21 * -num14 + value.M22 * num13;

                result = new Matrix4D
                (
                    num7 * num12,
                    -num19 * num12,
                    num23 * num12,
                    -num27 * num12,
                    -num8 * num12,
                    num20 * num12,
                    -num24 * num12,
                    num28 * num12,
                    num9 * num12,
                    -num21 * num12,
                    num25 * num12,
                    -num29 * num12,
                    -num10 * num12,
                    num22 * num12,
                    -num26 * num12,
                    num30 * num12
                );
            }
        }

        public Matrix4D Inverse()
        {
            Inverse(this, out var result);
            return result;
        }

        public static bool AlmostEqual(in Matrix4D left, in Matrix4D right, double maximumAbsoluteError)
        {
            return
                Math.Abs(left.M00 - right.M00) < maximumAbsoluteError &&
                Math.Abs(left.M01 - right.M01) < maximumAbsoluteError &&
                Math.Abs(left.M02 - right.M02) < maximumAbsoluteError &&
                Math.Abs(left.M03 - right.M03) < maximumAbsoluteError &&

                Math.Abs(left.M10 - right.M10) < maximumAbsoluteError &&
                Math.Abs(left.M11 - right.M11) < maximumAbsoluteError &&
                Math.Abs(left.M12 - right.M12) < maximumAbsoluteError &&
                Math.Abs(left.M13 - right.M13) < maximumAbsoluteError &&

                Math.Abs(left.M20 - right.M20) < maximumAbsoluteError &&
                Math.Abs(left.M21 - right.M21) < maximumAbsoluteError &&
                Math.Abs(left.M22 - right.M22) < maximumAbsoluteError &&
                Math.Abs(left.M23 - right.M23) < maximumAbsoluteError &&

                Math.Abs(left.M30 - right.M30) < maximumAbsoluteError &&
                Math.Abs(left.M31 - right.M31) < maximumAbsoluteError &&
                Math.Abs(left.M32 - right.M32) < maximumAbsoluteError &&
                Math.Abs(left.M33 - right.M33) < maximumAbsoluteError;
        }

        public bool AlmostEqual(in Matrix4D other, double maximumAbsoluteError)
        {
            return AlmostEqual(this, other, maximumAbsoluteError);
        }

        #endregion
    }
}
