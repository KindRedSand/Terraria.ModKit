using System;
using System.Globalization;

namespace Razorwing.Framework.Configuration
{
    public class BindableDouble : BindableNumber<double>
    {
        public override bool IsDefault => Math.Abs(Value - Default) < Precision;

        protected override double DefaultMinValue => double.MinValue;
        protected override double DefaultMaxValue => double.MaxValue;
        protected override double DefaultPrecision => double.Epsilon;

        public BindableDouble(double value = 0)
            : base(value)
        {
        }

        public override string ToString() => Value.ToString("0.0###", NumberFormatInfo.InvariantInfo);
    }
}
