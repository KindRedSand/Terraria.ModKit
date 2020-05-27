using System.Globalization;

namespace Razorwing.Framework.Configuration
{
    public class BindableInt : BindableNumber<int>
    {
        protected override int DefaultMinValue => int.MinValue;
        protected override int DefaultMaxValue => int.MaxValue;
        protected override int DefaultPrecision => 1;

        public BindableInt(int value = 0)
            : base(value)
        {
        }

        public override string ToString() => Value.ToString(NumberFormatInfo.InvariantInfo);
    }
}
