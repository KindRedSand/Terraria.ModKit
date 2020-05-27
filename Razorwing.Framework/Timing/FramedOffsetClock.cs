namespace Razorwing.Framework.Timing
{
    public class FramedOffsetClock : FramedClock
    {
        private double offset;

        public override double CurrentTime => base.CurrentTime + offset;

        public double Offset
        {
            get => offset;
            set
            {
                LastFrameTime += value - offset;
                offset = value;
            }
        }

        public FramedOffsetClock(IClock source)
            : base(source)
        {
        }
    }
}
