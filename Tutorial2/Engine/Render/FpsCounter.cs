using System;
using System.Diagnostics;

namespace RCi.Tutorials.Gfx.Engine.Render
{
    public class FpsCounter :
        IDisposable
    {
        #region // storage

        public TimeSpan UpdateRate { get; }
        private Stopwatch StopwatchUpdate { get; set; }
        private Stopwatch StopwatchFrame { get; set; }
        private TimeSpan Elapsed { get; set; }
        private int FrameCount { get; set; }
        public double FpsRender { get; private set; }
        public double FpsGlobal { get; private set; }
        public string FpsString => $"FPS = {FpsRender:0} ({FpsGlobal:0})";

        #endregion

        #region // ctor

        public FpsCounter(TimeSpan updateRate)
        {
            UpdateRate = updateRate;

            StopwatchUpdate = new Stopwatch();
            StopwatchFrame = new Stopwatch();

            StopwatchUpdate.Start();

            Elapsed = TimeSpan.Zero;
        }

        public void Dispose()
        {
            StopwatchUpdate?.Stop();
            StopwatchUpdate = default;

            StopwatchFrame?.Stop();
            StopwatchFrame = default;
        }

        #endregion

        #region // routines

        public void StartFrame()
        {
            StopwatchFrame.Restart();
        }

        public void StopFrame()
        {
            StopwatchFrame.Stop();
            Elapsed += StopwatchFrame.Elapsed;
            FrameCount++;

            var updateElapsed = StopwatchUpdate.Elapsed;
            if (updateElapsed >= UpdateRate)
            {
                FpsRender = FrameCount / Elapsed.TotalSeconds;
                FpsGlobal = FrameCount / updateElapsed.TotalSeconds;

                StopwatchUpdate.Restart();
                Elapsed = TimeSpan.Zero;
                FrameCount = 0;
            }
        }

        #endregion
    }
}
