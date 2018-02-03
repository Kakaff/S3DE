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
    }
}
