using System;

namespace S3DE.Maths
{
    public static class EngineMath
    {

        public static bool IsInrange(int min, int max, int value) => value >= min && value <= max;
        public static bool IsInrange(uint min, uint max, uint value) => value >= min && value <= max;
        public static bool IsInrange(byte min, byte max, byte value) => value >= min && value <= max;
        public static bool IsInrange(sbyte min, sbyte max, sbyte value) => value >= min && value <= max;

        public static bool IsInrange(short min, short max, short value) => value >= min && value <= max;
        public static bool IsInrange(ushort min, ushort max, ushort value) => value >= min && value <= max;

        public static bool IsInrange(float min, float max, float value) => value >= min && value <= max;
        public static bool IsInrange(double min, double max, double value) => value >= min && value <= max;
        
        public static float Normalize(float start, float end, float value)
        {
            float width = end - start;
            float offset = value - start;

            return (float)(offset - (System.Math.Floor(offset / width) * width)) + start;
        }

        public static long Normalize(long start, long end, long value) {
            long width = end - start;
            long offset = value - start;

            return (long)(offset - (Math.Floor((double)offset / (double)width) * width)) + start;
        }

        public static float Clamp(float min, float max, float value) => value > max ? max : value < min ? min : value;
        public static double Clamp(double min, double max, double value) => value > max ? max : value < min ? min : value;
        public static long Clamp(long min, long max, long value) => value > max ? max : value < min ? min : value;

        public static long MultipleOf(long value, int multiple) => (value / multiple) * multiple;
        public static double MultipleOf(double value, int multiple) => MultipleOf((long)value, multiple);

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
