using System;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using RCi.Tutorials.Gfx.Common.Camera;
using RCi.Tutorials.Gfx.Engine.Render;
using RCi.Tutorials.Gfx.Inputs;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Engine.Operators
{
    /// <summary>
    /// Camera pan operator.
    /// </summary>
    public class OperatorCameraOrbit :
        Operator
    {
        #region // storage

        private ICameraInfo MouseDownCameraInfo { get; set; }
        private Point3D? MouseDownView { get; set; }
        private Point3D? OrbitOrigin { get; set; }

        #endregion

        #region // ctor

        public OperatorCameraOrbit(IRenderHost renderHost) :
            base(renderHost)
        {
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            MouseDownCameraInfo = default;
            MouseDownView = default;
            OrbitOrigin = default;

            base.Dispose();
        }

        #endregion

        #region // routines

        /// <summary>
        /// Get orbit origin.
        /// </summary>
        private static Point3D GetOrbitOrigin(ICameraInfo cameraInfo)
        {
            return cameraInfo.Target;
        }

        /// <inheritdoc />
        protected override void InputOnMouseDown(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseDown(sender, args);

            if (args.Buttons != MouseButtons.Middle || args.ClickCount > 1) return;

            MouseDownCameraInfo = RenderHost.CameraInfo.Cloned();
            MouseDownView = MouseDownCameraInfo.GetTransformationMatrix(Space.Screen, Space.View).Transform(args.Position.ToPoint3D());
            OrbitOrigin = GetOrbitOrigin(MouseDownCameraInfo);
        }

        /// <inheritdoc />
        protected override void InputOnMouseUp(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseUp(sender, args);

            if (args.Buttons != MouseButtons.Middle) return;

            MouseDownCameraInfo = default;
            MouseDownView = default;
            OrbitOrigin = default;
        }

        /// <inheritdoc />
        protected override void InputOnMouseMove(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseMove(sender, args);

            if (!MouseDownView.HasValue || MouseDownCameraInfo == null || !OrbitOrigin.HasValue) return;

            var mouseMoveView = RenderHost.CameraInfo.GetTransformationMatrix(Space.Screen, Space.View).Transform(args.Position.ToPoint3D());
            RenderHost.CameraInfo = Orbit(MouseDownCameraInfo, mouseMoveView - MouseDownView.Value, OrbitOrigin.Value);
        }

        /// <summary>
        /// Create new <see cref="ICameraInfo"/> based on mouse offset in view space and orbit origin.
        /// </summary>
        public static ICameraInfo Orbit(ICameraInfo cameraInfoStart, Vector3D mouseOffsetView, Point3D orbitOrigin)
        {
            // default input
            var eye = cameraInfoStart.Position;
            var target = cameraInfoStart.Target;

            // create local coordinate system
            var zAxis = cameraInfoStart.UpVector;
            var yzPlane = new Plane(new Point3D(), cameraInfoStart.GetEyeDirection().ToPoint3D(), zAxis.ToPoint3D());
            var xAxis = yzPlane.Normal;
            var xzPlane = new Plane(new Point3D(), zAxis.ToPoint3D(), xAxis.ToPoint3D());
            var yAxis = xzPlane.Normal;
            var matrixWorldToLocal = Matrix4DEx.CoordinateSystem(new Point3D(), xAxis, yAxis, zAxis);

            // transform to local system
            orbitOrigin = matrixWorldToLocal.Transform(orbitOrigin);
            eye = matrixWorldToLocal.Transform(eye);
            target = matrixWorldToLocal.Transform(target);

            // figure out angles (how much to rotate)
            GetSphereAngles(mouseOffsetView, (target - eye).Normalize(), out var thetaDelta, out var phiDelta);

            // rotate horizontally
            var matrixRotationHorizontal = Matrix4DEx.Rotate(UnitVector3D.ZAxis, thetaDelta.Radians).TransformAround(orbitOrigin);
            eye = matrixRotationHorizontal.Transform(eye);
            target = matrixRotationHorizontal.Transform(target);

            // rotate vertically
            var phiPlane = new Plane(eye, target, target + UnitVector3D.ZAxis);
            var matrixRotationVertical = Matrix4DEx.Rotate(phiPlane.Normal, phiDelta.Radians).TransformAround(orbitOrigin);
            eye = matrixRotationVertical.Transform(eye);
            target = matrixRotationVertical.Transform(target);

            // transform back to world system
            var matrixLocalToWorld = matrixWorldToLocal.Inverse();
            eye = matrixLocalToWorld.Transform(eye);
            target = matrixLocalToWorld.Transform(target);

            // update camera info
            return new CameraInfo(eye, target, cameraInfoStart.UpVector, cameraInfoStart.Projection.Cloned(), cameraInfoStart.Viewport);
        }

        /// <summary>
        /// Get spherical angles for rotation based on mouse offset in view space. Clamps angle at default up vector (z = 0,0,1).
        /// </summary>
        private static void GetSphereAngles(Vector3D mouseOffsetView, UnitVector3D eyeDirection, out Angle thetaDelta, out Angle phiDelta)
        {
            // get deltas
            thetaDelta = new Angle(-mouseOffsetView.X * Math.PI, new Radians());  // horizontal (around z-axis)
            phiDelta = new Angle(mouseOffsetView.Y * Math.PI, new Radians());   // vertical

            var phiStart = UnitVector3D.ZAxis.AngleTo(-eyeDirection);
            var phiEnd = phiStart + phiDelta;

            // clamp phi so that new view vector won't match with upVector
            phiEnd = new Angle(Math.Max(Math.Min(phiEnd.Radians, Math.PI * 0.999), 0.001), new Radians());
            phiDelta = phiEnd - phiStart;
        }

        #endregion
    }
}
