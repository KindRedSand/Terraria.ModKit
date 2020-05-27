using System;
using Microsoft.Xna.Framework;

namespace Razorwing.Framework.Extensions.ColorExtensions
{
    public static class ColorExtensions
    {
        public const double GAMMA = 2.4;

        public static double ToLinear(double color)
        {
            return color <= 0.04045 ? color / 12.92 : Math.Pow((color + 0.055) / 1.055, GAMMA);
        }

        public static double ToSRGB(double color)
        {
            return color < 0.0031308 ? 12.92 * color : 1.055 * Math.Pow(color, 1.0 / GAMMA) - 0.055;
        }

        public static Color Opacity(this Color color, float a) => new Color(color.R, color.G, color.B, a);

        public static Color Opacity(this Color color, byte a) => new Color(color.R, color.G, color.B, a / 255f);

        public static Color ToLinear(this Color colour)
        {
            return new Color(
                (float)ToLinear(colour.R),
                (float)ToLinear(colour.G),
                (float)ToLinear(colour.B),
                colour.A);
        }

        public static Color ToSRGB(this Color colour)
        {
            return new Color(
                (float)ToSRGB(colour.R),
                (float)ToSRGB(colour.G),
                (float)ToSRGB(colour.B),
                colour.A);
        }

        public static Color MultiplySRGB(Color first, Color second)
        {
            if (first.Equals(Color.White))
                return second;

            if (second.Equals(Color.White))
                return first;

            first = first.ToLinear();
            second = second.ToLinear();

            return new Color(
                first.R * second.R,
                first.G * second.G,
                first.B * second.B,
                first.A * second.A).ToSRGB();
        }

        public static Color Multiply(Color first, Color second)
        {
            if (first.Equals(Color.White))
                return second;

            if (second.Equals(Color.White))
                return first;

            return new Color(
                first.R * second.R,
                first.G * second.G,
                first.B * second.B,
                first.A * second.A);
        }

        /// <summary>
        /// Returns a lightened version of the colour.
        /// </summary>
        /// <param name="colour">Original colour</param>
        /// <param name="amount">Decimal light addition</param>
        public static Color Lighten(this Color colour, float amount) => Multiply(colour, 1 + amount);

        /// <summary>
        /// Returns a darkened version of the colour.
        /// </summary>
        /// <param name="colour">Original colour</param>
        /// <param name="amount">Percentage light reduction</param>
        public static Color Darken(this Color colour, float amount) => Multiply(colour, 1 / (1 + amount));

        /// <summary>
        /// Multiply the RGB coordinates by a scalar.
        /// </summary>
        /// <param name="colour">Original colour</param>
        /// <param name="scalar">A scalar to multiply with</param>
        /// <returns></returns>
        public static Color Multiply(this Color colour, float scalar)
        {
            if (scalar < 0)
                throw new ArgumentOutOfRangeException(nameof(scalar), scalar, "Can not multiply colours by negative values.");

            return new Color(
                Math.Min(1, colour.R * scalar),
                Math.Min(1, colour.G * scalar),
                Math.Min(1, colour.B * scalar),
                colour.A);
        }
    }
}
