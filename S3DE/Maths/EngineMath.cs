using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths
{
    public static class EngineMath
    {
        public static float Normalize(float start, float end, float value)
        {
            float width = end - start;
            float offset = value - start;

            return (float)(offset - (System.Math.Floor(offset / width) * width)) + start;
        }

        public static float Clamp(float min, float max, float value) => value > max ? max : value < min ? min : value;
        public static double Clamp(double min, double max, double value) => value > max ? max : value < min ? min : value;

        public static bool IsPowerOfTwo(uint value) => ((value & ~(value - 1)) == value);

        public static int CeilToPowerOfTwo(int value)
        {
            if (value < 0)
                return -FloorToPowerOfTwo(-value);

            value--;
            value = value | (value >> 1);
            value = value | (value >> 2);
            value = value | (value >> 4);
            value = value | (value >> 8);
            value = value | (value >> 16);
            value++;
            return value;
        }

        public static int FloorToPowerOfTwo(int value)
        {
            if (value == 0)
                return value;
            if (value < 0)
                return -CeilToPowerOfTwo(-value);

            value = value | (value >> 1);
            value = value | (value >> 2);
            value = value | (value >> 4);
            value = value | (value >> 8);
            value = value | (value >> 16);
            return value - (value >> 1);
        }
    }
}
