using System;
using Microsoft.Xna.Framework;

namespace Razorwing.Graphics
{
    public class BaseColour
    {
        public static Color Gray(float amt) => new Color(amt, amt, amt, 1f);
        public static Color Gray(byte amt) => new Color(amt, amt, amt, 255);

        public static Color FromHex(string hex)
        {
            if (hex[0] == '#')
                hex = hex.Substring(1);

            switch (hex.Length)
            {
                default:
                    throw new ArgumentException(@"Invalid hex string length!");
                case 3:
                    return new Color(
                        (byte)(Convert.ToByte(hex.Substring(0, 1), 16) * 17),
                        (byte)(Convert.ToByte(hex.Substring(1, 1), 16) * 17),
                        (byte)(Convert.ToByte(hex.Substring(2, 1), 16) * 17),
                        255);
                case 6:
                    return new Color(
                        Convert.ToByte(hex.Substring(0, 2), 16),
                        Convert.ToByte(hex.Substring(2, 2), 16),
                        Convert.ToByte(hex.Substring(4, 2), 16),
                        255);
            }
        }

        public readonly Color PurpleLighter = FromHex(@"eeeeff");
        public readonly Color PurpleLight = FromHex(@"aa88ff");
        public readonly Color Purple = FromHex(@"8866ee");
        public readonly Color PurpleDark = FromHex(@"6644cc");
        public readonly Color PurpleDarker = FromHex(@"441188");

        public readonly Color PinkLighter = FromHex(@"ffddee");
        public readonly Color PinkLight = FromHex(@"ff99cc");
        public readonly Color Pink = FromHex(@"ff66aa");
        public readonly Color PinkDark = FromHex(@"cc5288");
        public readonly Color PinkDarker = FromHex(@"bb1177");

        public readonly Color BlueLighter = FromHex(@"ddffff");
        public readonly Color BlueLight = FromHex(@"99eeff");
        public readonly Color Blue = FromHex(@"66ccff");
        public readonly Color BlueDark = FromHex(@"44aadd");
        public readonly Color BlueDarker = FromHex(@"2299bb");

        public readonly Color YellowLighter = FromHex(@"ffffdd");
        public readonly Color YellowLight = FromHex(@"ffdd55");
        public readonly Color Yellow = FromHex(@"ffcc22");
        public readonly Color YellowDark = FromHex(@"eeaa00");
        public readonly Color YellowDarker = FromHex(@"cc6600");

        public readonly Color GreenLighter = FromHex(@"eeffcc");
        public readonly Color GreenLight = FromHex(@"b3d944");
        public readonly Color Green = FromHex(@"88b300");
        public readonly Color GreenDark = FromHex(@"668800");
        public readonly Color GreenDarker = FromHex(@"445500");

        public readonly Color Gray0 = FromHex(@"000");
        public readonly Color Gray1 = FromHex(@"111");
        public readonly Color Gray2 = FromHex(@"222");
        public readonly Color Gray3 = FromHex(@"333");
        public readonly Color Gray4 = FromHex(@"444");
        public readonly Color Gray5 = FromHex(@"555");
        public readonly Color Gray6 = FromHex(@"666");
        public readonly Color Gray7 = FromHex(@"777");
        public readonly Color Gray8 = FromHex(@"888");
        public readonly Color Gray9 = FromHex(@"999");
        public readonly Color GrayA = FromHex(@"aaa");
        public readonly Color GrayB = FromHex(@"bbb");
        public readonly Color GrayC = FromHex(@"ccc");
        public readonly Color GrayD = FromHex(@"ddd");
        public readonly Color GrayE = FromHex(@"eee");
        public readonly Color GrayF = FromHex(@"fff");

        public readonly Color RedLighter = FromHex(@"ffeded");
        public readonly Color RedLight = FromHex(@"ed7787");
        public readonly Color Red = FromHex(@"ed1121");
        public readonly Color RedDark = FromHex(@"ba0011");
        public readonly Color RedDarker = FromHex(@"870000");

        public readonly Color ChatBlue = FromHex(@"17292e");

        public readonly Color ContextMenuGray = FromHex(@"223034");
    }
}
