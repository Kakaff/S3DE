using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths
{
    public struct Vector3
    {
        float _x, _y, _z;

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

        public float z
        {
            get => _x;
            set => _z = value;
        }

        public float[] ToArray() => new float[] {_x,_y,_z};

        public float length => Length(this);
        
        public float lengthSquared => LengthSquared(this);

        public Vector3 normalized => Normalized(this);

        public static Vector3 Zero => new Vector3(0, 0, 0);
        public static Vector3 One => new Vector3(1, 1, 1);

        public static Vector3 Right => new Vector3(1, 0, 0);
        public static Vector3 Left => -Right;

        public static Vector3 Up => new Vector3(0, 1, 0);
        public static Vector3 Down => -Up;

        public static Vector3 Forward => new Vector3(0, 0, 1);
        public static Vector3 Backward => -Forward;

        public Vector3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public static float Dot(Vector3 v1, Vector3 v2) => v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.y * v2.z - v1.z * v2.y,
                               v1.z * v2.x - v1.x * v2.z,
                               v1.x * v2.y - v1.y * v2.x);
        }

        public static float Length(Vector3 v) => (float)Math.Sqrt(LengthSquared(v));

        public static float LengthSquared(Vector3 v) => v.x * v.x + v.y * v.y + v.z * v.z;

        public static Vector3 Normalized(Vector3 v) => v / Length(v);

        public Vector3 Transform(Matrix4x4 m)
        {
            return new Vector3(
                x * m[0, 0] + y * m[0, 1] + z * m[0, 2] + m[0, 3],
                x * m[1, 0] + y * m[1, 1] + z * m[1, 2] + m[1, 3],
                x * m[2, 0] + y * m[2, 1] + z * m[2, 2] + m[2, 3]
                );
        }

        public Vector3 Transform(Quaternion q)
        {
            Quaternion r = q * this * q.conjugate;

            return new Vector3(r.x,r.y,r.z);
        }


        public static Vector3 DirectionTo(Vector3 start, Vector3 target) => (target - start).normalized;

        public static Vector3 operator * (Vector3 vec, float f)
        {
            return new Vector3(vec.x * f, vec.y * f, vec.z * f);
        }

        public static Vector3 operator / (Vector3 vec, float f)
        {
            return new Vector3(vec.x / f, vec.y / f, vec.z / f);
        }

        public static Vector3 operator + (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator - (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static bool operator == (Vector3 v1, Vector3 v2)
        {
            if (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z)
                return true;
            else
                return false;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2) => !(v1 == v2);

        public static Vector3 operator -(Vector3 v) => new Vector3(-v.x, -v.y, -v.z);

        public static Vector3 operator * (Vector3 v, Matrix4x4 m)
        {
            return v.Transform(m);
        }

        public static Vector3 operator *(Vector3 v, Quaternion q)
        {
            return v.Transform(q);
        }

    }
}
