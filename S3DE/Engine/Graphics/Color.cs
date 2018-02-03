using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public struct Color
    {
        byte r, g, b, a;

        public byte R => r;
        public byte G => g;
        public byte B => b;
        public byte A => a;

        public Color(byte Red, byte Green, byte Blue, byte Alpha)
        {
            r = Red;
            g = Green;
            b = Blue;
            a = Alpha;
        }

        public byte[] ToArray() => new byte[] { r, g, b, a };
        public static Color White => new Color(255, 255, 255, 255);
        public static Color Gray => new Color(122, 122, 122, 255);
        public static Color Black => new Color(0, 0, 0, 255);
        public static Color Blue => new Color(0, 0, 255, 255);
        public static Color Red => new Color(255, 0, 0, 255);
        public static Color Green => new Color(0, 255, 0, 255);
    }
}
