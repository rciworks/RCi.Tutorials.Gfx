using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Common.Camera
{
    /// <summary>
    /// <see cref="ICameraInfo"/> extensions.
    /// </summary>
    public static class ICameraInfoEx
    {
        /// <summary>
        /// Vector from camera position to its target.
        /// </summary>
        public static Vector3D GetEyeVector(this ICameraInfo cameraInfo) => cameraInfo.Target - cameraInfo.Position;

        /// <summary>
        /// <see cref="GetEyeVector"/> normalized.
        /// </summary>
        public static UnitVector3D GetEyeDirection(this ICameraInfo cameraInfo) => cameraInfo.GetEyeVector().Normalize();

        /// <summary>
        /// Get transformation matrix from one space to another.
        /// </summary>
        public static Matrix<double> GetTransformationMatrix(this ICameraInfo cameraInfo, Space from, Space to)
        {
            switch (from)
            {
                case Space.World:
                    switch (to)
                    {
                        case Space.World:
                            return MatrixEx.Identity;

                        case Space.View:
                            return cameraInfo.Cache.MatrixViewProjection;

                        case Space.Screen:
                            return cameraInfo.Cache.MatrixViewProjectionViewport;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(to), to, null);
                    }

                case Space.View:
                    switch (to)
                    {
                        case Space.World:
                            return cameraInfo.Cache.MatrixViewProjectionInverse;

                        case Space.View:
                            return MatrixEx.Identity;

                        case Space.Screen:
                            return cameraInfo.Cache.MatrixViewport;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(to), to, null);
                    }

                case Space.Screen:
                    switch (to)
                    {
                        case Space.World:
                            return cameraInfo.Cache.MatrixViewProjectionViewportInverse;

                        case Space.View:
                            return cameraInfo.Cache.MatrixViewportInverse;

                        case Space.Screen:
                            return MatrixEx.Identity;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(to), to, null);
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(from), from, null);
            }
        }

        /// <inheritdoc cref="Projections.IProjection.GetMouseRay"/>
        public static Ray3D GetMouseRay(this ICameraInfo cameraInfo, Point3D mouseWorld)
        {
            return cameraInfo.Projection.GetMouseRay(cameraInfo, mouseWorld);
        }

        /// <inheritdoc cref="Projections.IProjection.GetMouseRay"/>
        public static Ray3D GetMouseRay(this ICameraInfo cameraInfo, Space space, Point3D mouseSpace)
        {
            return cameraInfo.GetMouseRay(cameraInfo.GetTransformationMatrix(space, Space.World).Transform(mouseSpace));
        }
    }
}
