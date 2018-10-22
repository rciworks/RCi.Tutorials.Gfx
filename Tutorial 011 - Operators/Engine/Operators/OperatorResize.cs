using System;
using System.Drawing;
using RCi.Tutorials.Gfx.Common.Camera;
using RCi.Tutorials.Gfx.Engine.Render;
using RCi.Tutorials.Gfx.Inputs;

namespace RCi.Tutorials.Gfx.Engine.Operators
{
    /// <summary>
    /// Operator responsible for resizing.
    /// </summary>
    public class OperatorResize :
        Operator
    {
        #region // storage

        private Action<Size> ResizeHost { get; set; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public OperatorResize(IRenderHost renderHost, Action<Size> resizeHost) :
            base(renderHost)
        {
            ResizeHost = resizeHost;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            ResizeHost = default;

            base.Dispose();
        }

        #endregion

        #region // routines

        protected override void InputOnSizeChanged(object sender, ISizeEventArgs args)
        {
            base.InputOnSizeChanged(sender, args);

            Resize(RenderHost, args.NewSize, ResizeHost);
        }

        public static void Resize(IRenderHost renderHost, Size hostSize, Action<Size> resizeHost)
        {
            // sanitize
            if (hostSize.Width < 1 || hostSize.Height < 1)
            {
                hostSize = new Size(1, 1);
            }

            // update host (surface size)
            if (renderHost.HostSize != hostSize)
            {
                resizeHost(hostSize);
            }

            // update camera info
            var cameraInfo = renderHost.CameraInfo;
            if (cameraInfo.Viewport.Size != hostSize)
            {
                var viewport = new Viewport
                (
                    cameraInfo.Viewport.X,
                    cameraInfo.Viewport.Y,
                    hostSize.Width,
                    hostSize.Height,
                    cameraInfo.Viewport.MinZ,
                    cameraInfo.Viewport.MaxZ
                );
                renderHost.CameraInfo = new CameraInfo
                (
                    cameraInfo.Position,
                    cameraInfo.Target,
                    cameraInfo.UpVector,
                    cameraInfo.Projection.GetAdjustedProjection(viewport.AspectRatio),
                    viewport
                );
            }
        }

        #endregion
    }
}
