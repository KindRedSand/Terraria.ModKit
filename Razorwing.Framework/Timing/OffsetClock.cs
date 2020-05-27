namespace Razorwing.Framework.Timing
{
    public class OffsetClock : IClock
    {
        protected IClock Source;

        public double Offset;

        public double CurrentTime => Source.CurrentTime + Offset;

        public double Rate => Source.Rate;

        public bool IsRunning => Source.IsRunning;

        public OffsetClock(IClock source)
        {
            Source = source;
        }
    }
}
