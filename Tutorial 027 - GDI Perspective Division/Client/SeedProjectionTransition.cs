using System;
using RCi.Tutorials.Gfx.Common.Camera;
using RCi.Tutorials.Gfx.Common.Camera.Projections;
using RCi.Tutorials.Gfx.Engine.Render;

namespace RCi.Tutorials.Gfx.Client
{
    public static class SeedProjectionTransition
    {
        /// <summary>
        /// Switch <see cref="ICameraInfo.Projection"/> of given <see cref="RenderHost"/>.
        /// </summary>
        public static void Switch(IRenderHost renderHost)
        {
            // get new projection
            IProjection projectionNew;
            switch (renderHost.CameraInfo.Projection)
            {
                case IProjectionOrthographic po:
                    // orthographic -> perspective
                    projectionNew = new ProjectionPerspective
                    (
                        po.NearPlane,
                        po.FarPlane,
                        Math.PI / 2,
                        renderHost.CameraInfo.Viewport.AspectRatio
                    );
                    break;

                case IProjectionPerspective pp:
                    // perspective -> orthographic
                    projectionNew = ProjectionOrthographic.FromDistance
                    (
                        pp.NearPlane,
                        pp.FarPlane,
                        (renderHost.CameraInfo.Target - renderHost.CameraInfo.Position).Length,
                        renderHost.CameraInfo.Viewport.AspectRatio
                    );
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(renderHost.CameraInfo.Projection));
            }

            // set new camera info
            renderHost.CameraInfo = new CameraInfo
            (
                renderHost.CameraInfo.Position,
                renderHost.CameraInfo.Target,
                renderHost.CameraInfo.UpVector,
                projectionNew,
                renderHost.CameraInfo.Viewport
            );
        }
    }
}
