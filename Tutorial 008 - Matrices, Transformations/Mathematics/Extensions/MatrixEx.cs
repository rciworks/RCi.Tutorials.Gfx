using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Engine.Common;

namespace RCi.Tutorials.Gfx.Mathematics.Extensions
{
    public static class MatrixEx
    {
        #region // import

        public static Matrix<double> ToMatrix(this double[] values, int rows, int columns)
        {
            return Matrix<double>.Build.DenseOfRowMajor(rows, columns, values);
        }

        public static Matrix<double> ToMatrix(this double[] values)
        {
            return ToMatrix(values, 4, 4);
        }

        public static Matrix<double> ToMatrix(this float[] values)
        {
            return ToMatrix(values.Select(f => (double)f).ToArray());
        }

        #endregion

        #region // export

        public static double[] ToDoublesRowMajor(this Matrix<double> matrix)
        {
            return matrix.AsRowMajorArray() ?? matrix.ToRowMajorArray();
        }

        public static double[] ToDoublesColumnMajor(this Matrix<double> matrix)
        {
            return matrix.AsColumnMajorArray() ?? matrix.ToColumnMajorArray();
        }

        public static double[] ToDoubles(this Matrix<double> matrix)
        {
            return matrix.ToDoublesRowMajor();
        }

        public static float[] ToFloatsRowMajor(this Matrix<double> matrix)
        {
            return matrix.ToDoublesRowMajor().Select(d => (float)d).ToArray();
        }

        public static float[] ToFloatsColumnMajor(this Matrix<double> matrix)
        {
            return matrix.ToDoublesColumnMajor().Select(d => (float)d).ToArray();
        }

        public static float[] ToFloats(this Matrix<double> matrix)
        {
            return matrix.ToDoubles().Select(d => (float)d).ToArray();
        }

        #endregion

        #region // identity

        private static Matrix<double> s_Identity { get; } =
            new double[]
            {
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1,
            }.ToMatrix();

        public static Matrix<double> Identity => s_Identity.Clone();

        #endregion

        #region // transform

        public static void MultiplyRowMajor(this Matrix<double> m,
            double x, double y, double z, double w,
            out double _x, out double _y, out double _z, out double _w)
        {
            _x = m[0, 0] * x + m[1, 0] * y + m[2, 0] * z + m[3, 0] * w;
            _y = m[0, 1] * x + m[1, 1] * y + m[2, 1] * z + m[3, 1] * w;
            _z = m[0, 2] * x + m[1, 2] * y + m[2, 2] * z + m[3, 2] * w;
            _w = m[0, 3] * x + m[1, 3] * y + m[2, 3] * z + m[3, 3] * w;
        }

        public static void MultiplyColumnMajor(this Matrix<double> m,
            double x, double y, double z, double w,
            out double _x, out double _y, out double _z, out double _w)
        {
            _x = m[0, 0] * x + m[0, 1] * y + m[0, 2] * z + m[0, 3] * w;
            _y = m[1, 0] * x + m[1, 1] * y + m[1, 2] * z + m[1, 3] * w;
            _z = m[2, 0] * x + m[2, 1] * y + m[2, 2] * z + m[2, 3] * w;
            _w = m[3, 0] * x + m[3, 1] * y + m[3, 2] * z + m[3, 3] * w;
        }

        public static Point3D Transform(this Matrix<double> m, in Point3D v)
        {
            MultiplyRowMajor(m, v.X, v.Y, v.Z, 1, out var x, out var y, out var z, out var w);
            return new Point3D(x / w, y / w, z / w);
        }

        public static Vector3D Transform(this Matrix<double> m, in Vector3D v)
        {
            MultiplyRowMajor(m, v.X, v.Y, v.Z, 1, out var x, out var y, out var z, out var w);
            return new Vector3D(x / w, y / w, z / w);
        }

        public static Vector3D Transform(this Matrix<double> m, in UnitVector3D v)
        {
            MultiplyRowMajor(m, v.X, v.Y, v.Z, 1, out var x, out var y, out var z, out var w);
            return new Vector3D(x / w, y / w, z / w);
        }

        public static IEnumerable<Point3D> Transform(this Matrix<double> matrix, IEnumerable<Point3D> value)
        {
            return value.Select(v => matrix.Transform(v));
        }

        public static IEnumerable<Vector3D> Transform(this Matrix<double> matrix, IEnumerable<Vector3D> value)
        {
            return value.Select(v => matrix.Transform(v));
        }

        public static void Transform(this Matrix<double> matrix, ref Point3D[] value)
        {
            for (var i = 0; i < value.Length; i++)
            {
                ref var reference = ref value[i];
                reference = matrix.Transform(reference);
            }
        }

        public static void Transform(this Matrix<double> matrix, ref Vector3D[] value)
        {
            for (var i = 0; i < value.Length; i++)
            {
                ref var reference = ref value[i];
                reference = matrix.Transform(reference);
            }
        }

        #endregion

        #region // transformations

        #region // scale

        public static Matrix<double> Scale(double x, double y, double z)
        {
            return new[]
            {
                x, 0, 0, 0,
                0, y, 0, 0,
                0, 0, z, 0,
                0, 0, 0, 1,
            }.ToMatrix();
        }

        public static Matrix<double> Scale(double uniform)
        {
            return Scale(uniform, uniform, uniform);
        }

        public static Matrix<double> Scale(in Point3D value)
        {
            return Scale(value.X, value.Y, value.Z);
        }

        public static Matrix<double> Scale(in Vector3D value)
        {
            return Scale(value.X, value.Y, value.Z);
        }

        #endregion

        #region // rotate

        public static Matrix<double> Rotate(in UnitVector3D axis, double angle)
        {
            var x = axis.X;
            var y = axis.Y;
            var z = axis.Z;
            var cosAngle = Math.Cos(angle);
            var sinAngle = Math.Sin(angle);
            var xx = x * x;
            var yy = y * y;
            var zz = z * z;
            var xy = x * y;
            var xz = x * z;
            var yz = y * z;

            return new[]
            {
                xx + cosAngle * (1 - xx),
                xy - cosAngle * xy + sinAngle * z,
                xz - cosAngle * xz - sinAngle * y,
                0,
                xy - cosAngle * xy - sinAngle * z,
                yy + cosAngle * (1 - yy),
                yz - cosAngle * yz + sinAngle * x,
                0,
                xz - cosAngle * xz + sinAngle * y,
                yz - cosAngle * yz - sinAngle * x,
                zz + cosAngle * (1 - zz),
                0,
                0, 0, 0, 1,
            }.ToMatrix();
        }

        public static Matrix<double> Rotate(in Vector3D axis, double angle)
        {
            return Rotate(axis.Normalize(), angle);
        }

        public static Matrix<double> Rotate(in Quaternion rotation)
        {
            var xx = rotation.ImagX * rotation.ImagX;
            var yy = rotation.ImagY * rotation.ImagY;
            var zz = rotation.ImagZ * rotation.ImagZ;
            var xy = rotation.ImagX * rotation.ImagY;
            var zw = rotation.ImagZ * rotation.Real;
            var xz = rotation.ImagX * rotation.ImagZ;
            var yw = rotation.ImagY * rotation.Real;
            var yz = rotation.ImagY * rotation.ImagZ;
            var xr = rotation.ImagX * rotation.Real;

            return new[]
            {
                1 - 2 * (yy + zz),
                2 * (xy + zw),
                2 * (xz - yw),
                0,
                2 * (xy - zw),
                1 - 2 * (zz + xx),
                2 * (yz + xr),
                0,
                2 * (xz + yw),
                2 * (yz - xr),
                1 - 2 * (yy + xx),
                0,
                0, 0, 0, 1,
            }.ToMatrix();
        }

        public static Quaternion ToQuaternion(this Matrix<double> matrix)
        {
            double x, y, z, w;

            var num1 = matrix[0, 0] + matrix[1, 1] + matrix[2, 2];
            if (num1 > 0.0)
            {
                var num2 = Math.Sqrt(num1 + 1);
                w = num2 * 0.5;
                var num3 = 0.5 / num2;
                x = (matrix[1, 2] - matrix[2, 1]) * num3;
                y = (matrix[2, 0] - matrix[0, 2]) * num3;
                z = (matrix[0, 1] - matrix[1, 0]) * num3;
            }
            else if (matrix[0, 0] >= matrix[1, 1] && matrix[0, 0] >= matrix[2, 2])
            {
                var num2 = Math.Sqrt(1.0 + matrix[0, 0] - matrix[1, 1] - matrix[2, 2]);
                var num3 = 0.5 / num2;
                x = 0.5 * num2;
                y = (matrix[0, 1] + matrix[1, 0]) * num3;
                z = (matrix[0, 2] + matrix[2, 0]) * num3;
                w = (matrix[1, 2] - matrix[2, 1]) * num3;
            }
            else if (matrix[1, 1] > matrix[2, 2])
            {
                var num2 = Math.Sqrt(1.0 + matrix[1, 1] - matrix[0, 0] - matrix[2, 2]);
                var num3 = 0.5 / num2;
                x = (matrix[1, 0] + matrix[0, 1]) * num3;
                y = 0.5 * num2;
                z = (matrix[2, 1] + matrix[1, 2]) * num3;
                w = (matrix[2, 0] - matrix[0, 2]) * num3;
            }
            else
            {
                var num2 = Math.Sqrt(1.0 + matrix[2, 2] - matrix[0, 0] - matrix[1, 1]);
                var num3 = 0.5 / num2;
                x = (matrix[2, 0] + matrix[0, 2]) * num3;
                y = (matrix[2, 1] + matrix[1, 2]) * num3;
                z = 0.5 * num2;
                w = (matrix[0, 1] - matrix[1, 0]) * num3;
            }

            return new Quaternion(w, x, y, z);
        }

        #endregion

        #region // translate

        public static Matrix<double> Translate(double x, double y, double z)
        {
            return new[]
            {
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                x, y, z, 1,
            }.ToMatrix();
        }

        public static Matrix<double> Translate(in Point3D value)
        {
            return Translate(value.X, value.Y, value.Z);
        }

        public static Matrix<double> Translate(in Vector3D value)
        {
            return Translate(value.X, value.Y, value.Z);
        }

        public static Matrix<double> TransformAround(this Matrix<double> transformation, in Point3D transformationOrigin)
        {
            var translate = Translate(transformationOrigin);
            return translate.Inverse() * transformation * translate;
        }

        #endregion

        #endregion

        #region // gfx

        /// <summary>
        /// World space to View space
        /// </summary>
        public static Matrix<double> LookAtRH(in Vector3D cameraPosition, in Vector3D cameraTarget, in UnitVector3D cameraUpVector)
        {
            var zaxis = (cameraPosition - cameraTarget).Normalize();
            var xaxis = cameraUpVector.CrossProduct(zaxis);
            var yaxis = zaxis.CrossProduct(xaxis);
            return new[]
            {
                xaxis.X, yaxis.X, zaxis.X, 0,
                xaxis.Y, yaxis.Y, zaxis.Y, 0,
                xaxis.Z, yaxis.Z, zaxis.Z, 0,
                -xaxis.DotProduct(cameraPosition), -yaxis.DotProduct(cameraPosition), -zaxis.DotProduct(cameraPosition), 1,
            }.ToMatrix();
        }

        /// <summary>
        /// View space to Clip space.
        /// </summary>
        public static Matrix<double> PerspectiveFovRH(double fieldOfViewY, double aspectRatio, double znearPlane, double zfarPlane)
        {
            var h = 1 / Math.Tan(fieldOfViewY * 0.5);
            var w = h / aspectRatio;
            return new[]
            {
                w, 0, 0, 0,
                0, h, 0, 0,
                0, 0, zfarPlane / (znearPlane - zfarPlane), -1,
                0, 0, znearPlane * zfarPlane / (znearPlane - zfarPlane), 0,
            }.ToMatrix();
        }

        /// <summary>
        /// View space to Clip space.
        /// </summary>
        public static Matrix<double> OrthoRH(double width, double height, double znearPlane, double zfarPlane)
        {
            return new[]
            {
                2 / width, 0, 0, 0,
                0, 2 / height, 0, 0,
                0, 0, 1 / (znearPlane - zfarPlane), 0,
                0, 0, znearPlane / (znearPlane - zfarPlane), 1,
            }.ToMatrix();
        }

        /// <summary>
        /// NDC to Screen space.
        /// </summary>
        public static Matrix<double> Viewport(in Viewport viewport)
        {
            return new[]
            {
                viewport.Width * 0.5, 0, 0, 0,
                0, -viewport.Height * 0.5, 0, 0,
                0, 0, viewport.MaxZ - viewport.MinZ, 0,
                viewport.X + viewport.Width * 0.5, viewport.Y + viewport.Height * 0.5, viewport.MinZ, 1,
            }.ToMatrix();
        }

        #endregion
    }
}
