using System.Globalization;

namespace Razorwing.Framework.Configuration
{
    public class BindableLong : BindableNumber<long>
    {
        protected override long DefaultMinValue => long.MinValue;
        protected override long DefaultMaxValue => long.MaxValue;
        protected override long DefaultPrecision => 1;

        public BindableLong(long value = 0)
            : base(value)
        {
        }

        public override string ToString() => Value.ToString(NumberFormatInfo.InvariantInfo);
    }
}
