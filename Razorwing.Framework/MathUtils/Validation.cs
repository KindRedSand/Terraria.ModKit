namespace Razorwing.Framework.MathUtils
{
    public static class Validation
    {
        /// <summary>
        /// Returns the exponent of a (single-precision) <see cref="float"/> as byte.
        /// </summary>
        /// <param name="value">The <see cref="float"/> to get the exponent from.</param>
        /// <remarks>Returns a <see cref="byte"/> so it's a smaller data type (and faster to pass around).</remarks>
        /// <returns>The exponent (bit 2 to 8) of the single-point <see cref="float"/>.</returns>
        private static unsafe byte singleToExponentAsByte(float value) => (byte)(*(int*)&value >> 23);

        /// <summary>
        /// Returns whether a value is not <see cref="float.NegativeInfinity"/>, <see cref="float.PositiveInfinity"/> or <see cref="float.NaN"/>.
        /// </summary>
        /// <param name="toCheck"></param>
        /// <remarks>Is equivalent to (<see cref="float.IsNaN(float)"/> || <see cref="float.IsInfinity(float)"/>), but with less overhead.</remarks>
        /// <returns>Whether the float is valid in our conditions.</returns>
        public static bool IsFinite(float toCheck) => singleToExponentAsByte(toCheck) != byte.MaxValue;
    }
}
