using System;
using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Common.Camera;

namespace RCi.Tutorials.Gfx.Mathematics.Extensions
{
    public static class Matrix4DEx
    {
        #region // convert

        public static double[] ToDoubles(this in Matrix4D value)
        {
            return new[]
            {
                value.M00, value.M01, value.M02, value.M03,
                value.M10, value.M11, value.M12, value.M13,
                value.M20, value.M21, value.M22, value.M23,
                value.M30, value.M31, value.M32, value.M33,
            };
        }

        public static float[] ToFloats(this in Matrix4D value)
        {
            return new[]
            {
                (float)value.M00, (float)value.M01, (float)value.M02, (float)value.M03,
                (float)value.M10, (float)value.M11, (float)value.M12, (float)value.M13,
                (float)value.M20, (float)value.M21, (float)value.M22, (float)value.M23,
                (float)value.M30, (float)value.M31, (float)value.M32, (float)value.M33,
            };
        }

        public static double[] ToDoublesColumnMajor(this in Matrix4D value)
        {
            return new[]
            {
                value.M00, value.M10, value.M20, value.M30,
                value.M01, value.M11, value.M21, value.M31,
                value.M02, value.M12, value.M22, value.M32,
                value.M03, value.M13, value.M23, value.M33,
            };
        }

        public static float[] ToFloatsColumnMajor(this in Matrix4D value)
        {
            return new[]
            {
                (float)value.M00, (float)value.M10, (float)value.M20, (float)value.M30,
                (float)value.M01, (float)value.M11, (float)value.M21, (float)value.M31,
                (float)value.M02, (float)value.M12, (float)value.M22, (float)value.M32,
                (float)value.M03, (float)value.M13, (float)value.M23, (float)value.M33,
            };
        }

        public static Matrix4D ToMatrix4D(this MathNet.Numerics.LinearAlgebra.Matrix<double> value)
        {
            return new Matrix4D
            (
                value[0, 0], value[0, 1], value[0, 2], value[0, 3],
                value[1, 0], value[1, 1], value[1, 2], value[1, 3],
                value[2, 0], value[2, 1], value[2, 2], value[2, 3],
                value[3, 0], value[3, 1], value[3, 2], value[3, 3]
            );
        }

        public static MathNet.Numerics.LinearAlgebra.Matrix<double> ToMatrix(this in Matrix4D value)
        {
            return MathNet.Numerics.LinearAlgebra.Matrix<double>.Build.DenseOfRowMajor(4, 4, value.ToDoubles());
        }

        #endregion

        #region // transform

        public static Vector4D Transform(this in Matrix4D m, in Vector4D v)
        {
            return m * v;
        }

        public static Vector3D Transform(this in Matrix4D m, in Vector3D v)
        {
            return m * v;
        }

        public static Vector3D Transform(this in Matrix4D m, in UnitVector3D v)
        {
            return m * v.ToVector3D();
        }

        public static Point3D Transform(this in Matrix4D m, in Point3D v)
        {
            return (m * v.ToVector3D()).ToPoint3D();
        }

        public static Vector4F Transform(this in Matrix4D m, in Vector4F v)
        {
            return (m * v.ToVector4D()).ToVector4F();
        }

        public static Vector3F Transform(this in Matrix4D m, Vector3F v)
        {
            return (m * v.ToVector3D()).ToVector3F();
        }

        public static Vector3F[] Transform(this in Matrix4D m, Vector3F[] vs)
        {
            var r = new Vector3F[vs.Length];
            for (var i = 0; i < vs.Length; i++)
            {
                r[i] = m.Transform(vs[i]);
            }
            return r;
        }

        #endregion

        #region // scale

        public static Matrix4D Scale(double x, double y, double z)
        {
            return new Matrix4D
            (
                x, 0, 0, 0,
                0, y, 0, 0,
                0, 0, z, 0,
                0, 0, 0, 1
            );
        }

        public static Matrix4D Scale(double uniform)
        {
            return Scale(uniform, uniform, uniform);
        }

        public static Matrix4D Scale(in Vector3D value)
        {
            return Scale(value.X, value.Y, value.Z);
        }

        #endregion

        #region // rotate

        public static Matrix4D Rotate(in UnitVector3D axis, double angle)
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

            return new Matrix4D
            (
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
                0,
                0,
                0,
                1
            );
        }

        public static Matrix4D Rotate(in Vector3D axis, double angle)
        {
            return Rotate(axis.Normalize(), angle);
        }

        public static Matrix4D Rotate(in Quaternion rotation)
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

            return new Matrix4D
            (
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
                0,
                0,
                0,
                1
            );
        }

        #endregion

        #region // translate

        public static Matrix4D Translate(double x, double y, double z)
        {
            return new Matrix4D
            (
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                x, y, z, 1
            );
        }

        public static Matrix4D Translate(in Point3D value)
        {
            return Translate(value.X, value.Y, value.Z);
        }

        public static Matrix4D Translate(in Vector3D value)
        {
            return Translate(value.X, value.Y, value.Z);
        }

        #endregion

        #region // custom

        public static Matrix4D TransformAround(this in Matrix4D transformation, in Point3D transformationOrigin)
        {
            var translate = Translate(transformationOrigin);
            return translate.Inverse() * transformation * translate;
        }

        public static Matrix4D CoordinateSystem(in Point3D origin, in UnitVector3D xAxis, in UnitVector3D yAxis, in UnitVector3D zAxis)
        {
            return new Matrix4D
            (
                xAxis.ToVector4D(0),
                yAxis.ToVector4D(0),
                zAxis.ToVector4D(0),
                origin.ToVector4D(1)
            );
        }

        #endregion

        #region // gfx

        /// <summary>
        /// World space to View space.
        /// </summary>
        public static Matrix4D LookAtRH(in Vector3D cameraPosition, in Vector3D cameraTarget, in UnitVector3D cameraUpVector)
        {
            var zaxis = (cameraPosition - cameraTarget).Normalize();
            var xaxis = cameraUpVector.CrossProduct(zaxis);
            var yaxis = zaxis.CrossProduct(xaxis);
            return new Matrix4D
            (
                xaxis.X, yaxis.X, zaxis.X, 0,
                xaxis.Y, yaxis.Y, zaxis.Y, 0,
                xaxis.Z, yaxis.Z, zaxis.Z, 0,
                -xaxis.DotProduct(cameraPosition), -yaxis.DotProduct(cameraPosition), -zaxis.DotProduct(cameraPosition), 1
            );
        }

        /// <summary>
        /// View space to Clip space.
        /// </summary>
        public static Matrix4D PerspectiveFovRH(double fieldOfViewY, double aspectRatio, double znearPlane, double zfarPlane)
        {
            var h = 1 / Math.Tan(fieldOfViewY * 0.5);
            var w = h / aspectRatio;
            return new Matrix4D
            (
                w, 0, 0, 0,
                0, h, 0, 0,
                0, 0, zfarPlane / (znearPlane - zfarPlane), -1,
                0, 0, znearPlane * zfarPlane / (znearPlane - zfarPlane), 0
            );
        }

        /// <summary>
        /// View space to Clip space.
        /// </summary>
        public static Matrix4D OrthoRH(double width, double height, double znearPlane, double zfarPlane)
        {
            return new Matrix4D
            (
                2 / width, 0, 0, 0,
                0, 2 / height, 0, 0,
                0, 0, 1 / (znearPlane - zfarPlane), 0,
                0, 0, znearPlane / (znearPlane - zfarPlane), 1
            );
        }

        /// <summary>
        /// Clip to Screen space.
        /// </summary>
        public static Matrix4D Viewport(in Viewport viewport)
        {
            return new Matrix4D
            (
                viewport.Width * 0.5, 0, 0, 0,
                0, -viewport.Height * 0.5, 0, 0,
                0, 0, viewport.MaxZ - viewport.MinZ, 0,
                viewport.X + viewport.Width * 0.5, viewport.Y + viewport.Height * 0.5, viewport.MinZ, 1
            );
        }

        #endregion
    }
}
