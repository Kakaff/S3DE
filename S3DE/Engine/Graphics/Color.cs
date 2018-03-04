using S3DE.Maths;
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

        public Color(byte Red, byte Green, byte Blue)
        {
            r = Red;
            g = Green;
            b = Blue;
            a = 255;
        }
        public Color(byte Red, byte Green, byte Blue, byte Alpha)
        {
            r = Red;
            g = Green;
            b = Blue;
            a = Alpha;
        }

        public Color(float Red, float Green, float Blue)
        {
            r = (byte)(EngineMath.Clamp(0, 1, Red) * 255);
            g = (byte)(EngineMath.Clamp(0, 1, Green) * 255);
            b = (byte)(EngineMath.Clamp(0, 1, Blue) * 255);
            a = 255;
        }

        public Color(float Red, float Green, float Blue, float Alpha)
        {
            r = (byte)(EngineMath.Clamp(0, 1, Red) * 255);
            g = (byte)(EngineMath.Clamp(0, 1, Green) * 255);
            b = (byte)(EngineMath.Clamp(0,1,  Blue) * 255);
            a = (byte)(EngineMath.Clamp(0, 1, Alpha) * 255);
        }

        public byte[] ToArray() => new byte[] { r, g, b, a };
        public static Color White => new Color(255, 255, 255, 255);
        public static Color Gray => new Color(122, 122, 122, 255);
        public static Color Black => new Color(0, 0, 0, 255);
        public static Color Blue => new Color(0, 0, 255, 255);
        public static Color Red => new Color(255, 0, 0, 255);
        public static Color Green => new Color(0, 255, 0, 255);

        public static implicit operator Color(System.Drawing.Color c) => new Color(c.R,c.G,c.B,c.A);
        public static implicit operator Vector3(Color c) => new Vector3(c.r / 255f,c.g / 255f,c.b / 255f);
    }
}
