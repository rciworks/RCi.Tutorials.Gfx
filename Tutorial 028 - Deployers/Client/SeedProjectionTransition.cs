using System;
using System.Threading;
using System.Threading.Tasks;
using RCi.Tutorials.Gfx.Common.Camera;
using RCi.Tutorials.Gfx.Common.Camera.Projections;
using RCi.Tutorials.Gfx.Engine.Render;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Client
{
    public static class SeedProjectionTransition
    {
        private static TimeSpan TRANSITION_DURATION { get; } = new TimeSpan(0, 0, 0, 0, 2000);

        private static TimeSpan TRANSITION_SLEEP { get; } = new TimeSpan(0, 0, 0, 0, 5);

        /// <summary>
        /// Activation function to normalize value.
        /// </summary>
        private static double Activate(double value)
        {
            // get non-linear alpha (using sin)
            var valueSin = Math.Sin(-(Math.PI * 0.5) + value * Math.PI);

            // normalize to [0..1]
            return (valueSin + 1) * 0.5;
        }

        /// <summary>
        /// Switch <see cref="ICameraInfo.Projection"/> of given <see cref="RenderHost"/>.
        /// </summary>
        public static void Switch(IRenderHost renderHost)
        {
            // get new projection
            switch (renderHost.CameraInfo.Projection)
            {
                case IProjectionOrthographic po:
                    // orthographic -> perspective
                    renderHost.CameraInfo = new CameraInfo
                    (
                        renderHost.CameraInfo.Position,
                        renderHost.CameraInfo.Target,
                        renderHost.CameraInfo.UpVector,
                        new ProjectionPerspective
                        (
                            po.NearPlane,
                            po.FarPlane,
                            Math.PI / 2,
                            renderHost.CameraInfo.Viewport.AspectRatio
                        ),
                        renderHost.CameraInfo.Viewport
                    );
                    break;

                case IProjectionPerspective pp:
                    // perspective -> orthographic
                    renderHost.CameraInfo = new CameraInfo
                    (
                        renderHost.CameraInfo.Position,
                        renderHost.CameraInfo.Target,
                        renderHost.CameraInfo.UpVector,
                        ProjectionOrthographic.FromDistance
                        (
                            pp.NearPlane,
                            pp.FarPlane,
                            (renderHost.CameraInfo.Target - renderHost.CameraInfo.Position).Length,
                            renderHost.CameraInfo.Viewport.AspectRatio
                        ),
                        renderHost.CameraInfo.Viewport
                    );
                    break;

                case IProjectionCombined _:
                    // do nothing, allow transition to finish
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(renderHost.CameraInfo.Projection));
            }
        }

        /// <summary>
        /// Launch and control transition to switch between <see cref="IProjectionCombined"/>.
        /// </summary>
        private static void LaunchTransition(IRenderHost renderHost)
        {
            // constants
            var synchronizationContextSTA = SynchronizationContext.Current;

            // cross-thread vars
            var projectionEndType = default(Type);
            var transitionStarted = DateTime.UtcNow;

            // launch transition
            Task.Factory.StartNew(() =>
            {
                var stop = false;
                while (!stop)
                {
                    synchronizationContextSTA.Send(state =>
                    {
                        // check if disposed
                        if (renderHost.CameraInfo is null)
                        {
                            return;
                        }

                        if (renderHost.CameraInfo.Projection is IProjectionCombined pc)
                        {
                            var dateTimeCurrent = DateTime.UtcNow;
                            var projectionEndTypeCurrent = pc.Projection1.GetType();

                            // get transition progress [0..1]
                            double GetTransitionProgress() => ((dateTimeCurrent - transitionStarted).TotalMilliseconds / TRANSITION_DURATION.TotalMilliseconds).Clamp(0, 1);

                            // ensure initial state on the first transition frame
                            if (projectionEndType is null)
                            {
                                projectionEndType = projectionEndTypeCurrent;
                            }

                            // check if flip happened during transition
                            if (projectionEndType != projectionEndTypeCurrent)
                            {
                                transitionStarted = dateTimeCurrent - new TimeSpan((long)(TRANSITION_DURATION.Ticks * (1.0 - GetTransitionProgress())));
                                projectionEndType = projectionEndTypeCurrent;
                            }

                            // get transition progress and figure out this frame combined projection
                            var progress = Activate(GetTransitionProgress());
                            if (progress < 1)
                            {
                                // still in transition (change alpha)
                                renderHost.CameraInfo = new CameraInfo
                                (
                                    renderHost.CameraInfo.Position,
                                    renderHost.CameraInfo.Target,
                                    renderHost.CameraInfo.UpVector,
                                    new ProjectionCombined(pc.Projection0, pc.Projection1, 1 - progress),
                                    renderHost.CameraInfo.Viewport
                                );
                            }
                            else
                            {
                                // transition ended, finalize
                                renderHost.CameraInfo = new CameraInfo
                                (
                                    renderHost.CameraInfo.Position,
                                    renderHost.CameraInfo.Target,
                                    renderHost.CameraInfo.UpVector,
                                    pc.Projection1,
                                    renderHost.CameraInfo.Viewport
                                );
                                stop = true;
                            }
                        }
                        else
                        {
                            stop = true;
                        }
                    }, null);

                    // allow message pump
                    Thread.Sleep(TRANSITION_SLEEP);
                }
            });
        }

        /// <summary>
        /// Launch transition to switch <see cref="ICameraInfo.Projection"/> of given <see cref="RenderHost"/>.
        /// </summary>
        public static void Transit(IRenderHost renderHost)
        {
            switch (renderHost.CameraInfo.Projection)
            {
                case IProjectionOrthographic po:
                    #region // orthographic -> perspective

                    var po2p = new ProjectionPerspective
                    (
                        po.NearPlane,
                        po.FarPlane,
                        Math.PI / 2,
                        renderHost.CameraInfo.Viewport.AspectRatio
                    );
                    renderHost.CameraInfo = new CameraInfo
                    (
                        renderHost.CameraInfo.Position,
                        renderHost.CameraInfo.Target,
                        renderHost.CameraInfo.UpVector,
                        new ProjectionCombined(po, po2p, 1),
                        renderHost.CameraInfo.Viewport
                    );
                    LaunchTransition(renderHost);
                    break;

                #endregion

                case IProjectionPerspective pp:
                    #region // perspective -> orthographic

                    var pp2o = ProjectionOrthographic.FromDistance
                    (
                        pp.NearPlane,
                        pp.FarPlane,
                        (renderHost.CameraInfo.Target - renderHost.CameraInfo.Position).Length,
                        renderHost.CameraInfo.Viewport.AspectRatio
                    );
                    renderHost.CameraInfo = new CameraInfo
                    (
                        renderHost.CameraInfo.Position,
                        renderHost.CameraInfo.Target,
                        renderHost.CameraInfo.UpVector,
                        new ProjectionCombined(pp, pp2o, 1),
                        renderHost.CameraInfo.Viewport
                    );
                    LaunchTransition(renderHost);
                    break;

                #endregion

                case IProjectionCombined pc:
                    #region // in transition at the moment -> flip direction

                    renderHost.CameraInfo = new CameraInfo
                    (
                        renderHost.CameraInfo.Position,
                        renderHost.CameraInfo.Target,
                        renderHost.CameraInfo.UpVector,
                        new ProjectionCombined(pc.Projection1, pc.Projection0, pc.Weight1),
                        renderHost.CameraInfo.Viewport
                    );
                    break;

                #endregion

                default:
                    throw new ArgumentOutOfRangeException(nameof(renderHost.CameraInfo.Projection));
            }
        }
    }
}
