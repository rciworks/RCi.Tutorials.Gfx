using RCi.Tutorials.Gfx.Common.Camera;
using RCi.Tutorials.Gfx.Common.Camera.Projections;
using RCi.Tutorials.Gfx.Engine.Render;
using RCi.Tutorials.Gfx.Inputs;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Engine.Operators
{
    /// <summary>
    /// Camera zoom operator.
    /// </summary>
    public class OperatorCameraZoom :
        Operator
    {
        #region // ctor

        /// <inheritdoc />
        public OperatorCameraZoom(IRenderHost renderHost) :
            base(renderHost)
        {
        }

        #endregion

        #region // routines

        protected override void InputOnMouseWheel(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseWheel(sender, args);

            // copy camera reference
            var cameraInfo = RenderHost.CameraInfo;

            // default scaling
            const double scale = 0.15;
            const double scaleForward = 1.0 + scale;
            const double scaleBackwards = 2.0 - 1.0 / (1.0 - scale);

            // calculate how much to zoom
            var scaleCurrent = args.WheelDelta > 0 /* scroll up => zoom in */ ? scaleForward : scaleBackwards;
            var eyeVector = cameraInfo.Target - cameraInfo.Position;
            var offset = eyeVector.ScaleBy(scaleCurrent) - eyeVector;

            // zoom
            var position = cameraInfo.Position + offset;

            var projection = cameraInfo.Projection is ProjectionOrthographic po
                // if projection is orthographic we need to resize view field
                ? ProjectionOrthographic.FromDistance(po.NearPlane, po.FarPlane, (cameraInfo.Target - position).Length, cameraInfo.Viewport.AspectRatio)
                // otherwise just clone existing one
                : cameraInfo.Projection.Cloned();

            // set camera info
            RenderHost.CameraInfo = new CameraInfo(position, cameraInfo.Target, cameraInfo.UpVector, projection, cameraInfo.Viewport);
        }

        #endregion
    }
}
