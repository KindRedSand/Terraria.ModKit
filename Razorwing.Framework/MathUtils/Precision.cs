using System;

namespace Razorwing.Framework.MathUtils
{
    public static class Precision
    {
        public const float FLOAT_EPSILON = 1e-3f;
        public const double DOUBLE_EPSILON = 1e-7;

        public static bool DefinitelyBigger(float value1, float value2, float acceptableDifference = FLOAT_EPSILON)
        {
            return value1 - acceptableDifference > value2;
        }

        public static bool DefinitelyBigger(double value1, double value2, double acceptableDifference = DOUBLE_EPSILON)
        {
            return value1 - acceptableDifference > value2;
        }

        public static bool AlmostBigger(float value1, float value2, float acceptableDifference = FLOAT_EPSILON)
        {
            return value1 > value2 - acceptableDifference;
        }

        public static bool AlmostBigger(double value1, double value2, double acceptableDifference = DOUBLE_EPSILON)
        {
            return value1 > value2 - acceptableDifference;
        }

        public static bool AlmostEquals(float value1, float value2, float acceptableDifference = FLOAT_EPSILON)
        {
            return Math.Abs(value1 - value2) <= acceptableDifference;
        }

        public static bool AlmostEquals(double value1, double value2, double acceptableDifference = DOUBLE_EPSILON)
        {
            return Math.Abs(value1 - value2) <= acceptableDifference;
        }
    }
}
