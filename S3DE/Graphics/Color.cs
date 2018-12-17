using S3DE.Maths;

namespace S3DE.Graphics
{
    public struct Color
    {
        byte r, g, b, a;

        public byte R { get => r; set => r = value; }
        public byte G { get => g; set => g = value; }
        public byte B { get => b; set => b = value; }
        public byte A { get => a; set => a = value; }

        public Color(byte red, byte green, byte blue, byte alpha)
        {
            this.r = red;
            this.g = green;
            this.b = blue;
            this.a = alpha;
        }

        public Color(byte red, byte green, byte blue)
        {
            r = red;
            g = green;
            b = blue;
            a = 255;
        }

        public override string ToString()
        {
            return $"(Red: {r}, Green: {g}, Blue: {b}, Alpha: {a})";
        }

        public static implicit operator Vector4(Color c)
        {
            return new Vector4(c.r, c.g, c.b, c.a) / 255f;
        }

        public static implicit operator Vector3(Color c)
        {
            return new Vector3(c.r, c.g, c.b) / 255f;
        }

        public static implicit operator Color(Vector4 v)
        {
            return new Color((byte)(EngineMath.Normalize(0, 1, v.x) * 255),
                (byte)(EngineMath.Normalize(0, 1, v.y) * 255),
                (byte)(EngineMath.Normalize(0, 1, v.z) * 255),
                (byte)(EngineMath.Normalize(0,1,v.w) * 255));
        }

        public static implicit operator Color(Vector3 v)
        {
            return new Color((byte)(EngineMath.Normalize(0, 1, v.x) * 255),
                (byte)(EngineMath.Normalize(0, 1, v.y) * 255),
                (byte)(EngineMath.Normalize(0, 1, v.z) * 255),
                1);
        }
    }
}
