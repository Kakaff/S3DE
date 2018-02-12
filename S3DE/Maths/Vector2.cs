using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths
{
    public struct Vector2
    {
        float _x;
        float _y;

        public float x
        {
            get => _x;
            set => _x = value;
        }

        public float y
        {
            get => _y;
            set => _y = value;
        }

        public static Vector2 Zero => new Vector2(0, 0);
        public static Vector2 One => new Vector2(1, 1);
        public float[] ToArray() => new float[] { _x, _y };

        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            return $"({x},{y})";
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2) => new Vector2(v1.x - v2.x, v1.y - v2.y);
        public static Vector2 operator +(Vector2 v1, Vector2 v2) => new Vector2(v1.x + v2.x, v1.y + v2.y);
        public static Vector2 operator /(Vector2 v1, Vector2 v2) => new Vector2(v1.x / v2.x, v1.y / v2.y);
        public static Vector2 operator *(Vector2 v1, Vector2 v2) => new Vector2(v1.x * v2.x, v1.y * v2.y);

        public static bool operator == (Vector2 v1, Vector2 v2)
        {
            if (v1.x == v2.x && v1.y == v2.y)
                return true;

            return false;
        }

        public static bool operator !=(Vector2 v1, Vector2 v2) => !(v1 == v2);
    }
}
