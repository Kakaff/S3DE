using System;
using System.Runtime.InteropServices;

namespace S3DE.Maths
{
    [StructLayout(LayoutKind.Sequential,Pack = 16)]
    public struct Vector3
    {
        public float x, y, z;
        
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 Up => new Vector3(0, 1, 0);
        public static Vector3 Right => new Vector3(1, 0, 0);
        public static Vector3 Forward => new Vector3(0, 0, -1);
        public static Vector3 Down => -Up;
        public static Vector3 Left => -Right;
        public static Vector3 Backward => -Forward;
        public static Vector3 One => new Vector3(1, 1, 1);
        public static Vector3 Zero => new Vector3(0, 0, 0);


        public Vector3 Transform(Matrix4x4 m) => m.Transform(this);
        public Vector3 Transform(Quaternion q) => q.Transform(this);

        public float Length()
        {
            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        public float LengthSquared()
        {
            return (x * x) + (y * y) + (z * z);
        }

        public Vector3 Normalized() => this / Length();
        public static Vector3 Normalize(Vector3 v) => v.Normalized();
        public Vector3 Cross(Vector3 v) => Vector3.Cross(this, v);

        public float SquaredNorm()
        {
            return Dot(this, this);
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.y * v2.z - v1.z * v2.y,
                v1.z * v2.x - v1.x * v2.z,
                v1.x * v2.y - v1.y * v2.x);
        }

        public static float Dot(Vector3 v1, Vector3 v2) => v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;

        public static Vector3 DirectionTo(Vector3 start, Vector3 target) => (target - start).Normalized();

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2) => !(v1 == v2);

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.x, -v.y, -v.z);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator / (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        public static Vector3 operator / (Vector3 v, float f)
        {
            return new Vector3(v.x / f, v.y / f, v.z / f);
        }

        public static unsafe Vector3 operator * (Vector3 v, float f)
        {
            return new Vector3(v.x * f, v.y * f, v.z * f);
            
        }

        public static Vector3 operator * (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public override string ToString()
        {
            return $"Vec3({x},{y},{z})";
        }
    }
}
