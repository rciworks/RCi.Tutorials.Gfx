using System;
using System.Collections.Generic;
using System.Drawing;
using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Common.Camera;
using RCi.Tutorials.Gfx.Common.Camera.Projections;
using RCi.Tutorials.Gfx.Engine.Operators;
using RCi.Tutorials.Gfx.Inputs;
using RCi.Tutorials.Gfx.Materials;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Engine.Render
{
    /// <summary>
    /// Base class for render host.
    /// </summary>
    public abstract class RenderHost :
        IRenderHost
    {
        #region // storage

        /// <inheritdoc />
        public IntPtr HostHandle { get; private set; }

        /// <inheritdoc />
        public IInput HostInput { get; private set; }

        /// <inheritdoc />
        public Size HostSize { get; private set; }

        /// <summary>
        /// Desired buffer size.
        /// </summary>
        protected Size BufferSize { get; private set; }

        /// <inheritdoc cref="CameraInfo" />
        private ICameraInfo m_CameraInfo;

        /// <inheritdoc />
        public ICameraInfo CameraInfo
        {
            get => m_CameraInfo;
            set
            {
                m_CameraInfo = value;
                CameraInfoChanged?.Invoke(this, m_CameraInfo);
            }
        }

        /// <summary>
        /// Active operators.
        /// </summary>
        protected IEnumerable<IOperator> Operators { get; set; }

        /// <inheritdoc />
        public FpsCounter FpsCounter { get; private set; }

        /// <summary>
        /// Timestamp when frame was started (UTC).
        /// </summary>
        protected DateTime FrameStarted { get; private set; }

        #endregion

        #region // events

        /// <inheritdoc />
        public event EventHandler<ICameraInfo> CameraInfoChanged;

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected RenderHost(IRenderHostSetup renderHostSetup)
        {
            HostHandle = renderHostSetup.HostHandle;
            HostInput = renderHostSetup.HostInput;

            HostSize = HostInput.Size;
            BufferSize = HostInput.Size;
            CameraInfo = new CameraInfo
            (
                new Point3D(1, 1, 1),
                new Point3D(0, 0, 0),
                new UnitVector3D(0, 0, 1),
                new ProjectionPerspective(0.001, 1000, Math.PI * 0.5, 1),
                //new ProjectionOrthographic(0.001, 1000, 2, 2),
                new Viewport(0, 0, 1, 1, 0, 1)
            );
            FpsCounter = new FpsCounter(new TimeSpan(0, 0, 0, 0, 1000));

            Operators = new List<IOperator>
            {
                new OperatorResize(this, ResizeHost),
                new OperatorCameraZoom(this),
                new OperatorCameraPan(this),
                new OperatorCameraOrbit(this),
            };

            OperatorResize.Resize(this, HostSize, ResizeHost);
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            Operators.ForEach(o => o.Dispose());
            Operators = default;

            FpsCounter.Dispose();
            FpsCounter = default;

            CameraInfo = default;
            BufferSize = default;
            HostSize = default;

            HostInput.Dispose();
            HostInput = default;

            HostHandle = default;
        }

        #endregion

        #region // routines

        /// <summary>
        /// Resize host.
        /// </summary>
        protected virtual void ResizeHost(Size size)
        {
            HostSize = size;
        }

        /// <summary>
        /// Resize all buffers.
        /// </summary>
        protected virtual void ResizeBuffers(Size size)
        {
            BufferSize = size;
        }

        /// <summary>
        /// Ensure <see cref="BufferSize"/> are synced with <see cref="ICameraInfo"/>
        /// </summary>
        protected void EnsureBufferSize()
        {
            var size = CameraInfo.Viewport.Size;
            if (BufferSize != size)
            {
                ResizeBuffers(size);
            }
        }

        #endregion

        #region // render

        /// <inheritdoc />
        public void Render(IEnumerable<IPrimitive> primitives)
        {
            EnsureBufferSize();
            FrameStarted = DateTime.UtcNow;
            FpsCounter.StartFrame();
            RenderInternal(primitives);
            FpsCounter.StopFrame();
        }

        /// <summary>
        /// Internal rendering for particular driver.
        /// </summary>
        protected abstract void RenderInternal(IEnumerable<IPrimitive> primitives);

        #endregion
    }
}
