namespace Razorwing.Framework.Timing
{
    /// <summary>
    /// A completely manual clock implementation. Everything is settable.
    /// </summary>
    public class ManualClock : IClock
    {
        public double CurrentTime { get; set; }
        public double Rate { get; set; }
        public bool IsRunning { get; set; }
    }
}
