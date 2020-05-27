namespace Razorwing.Framework.Timing
{
    /// <summary>
    /// A clock which will only update its current time when a frame proces is triggered.
    /// Useful for keeping a consistent time state across an individual update.
    /// </summary>
    public interface IFrameBasedClock : IClock
    {
        /// <summary>
        /// Elapsed time since last frame in milliseconds.
        /// </summary>
        double ElapsedFrameTime { get; }

        double FramesPerSecond { get; }

        FrameTimeInfo TimeInfo { get; }

        /// <summary>
        /// Processes one frame. Generally should be run once per update loop.
        /// </summary>
        void ProcessFrame();
    }
}
