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
            get => _z;
            set => _z = value;
        }

        public Vector2 xy => new Vector2(_x, _y);
        public Vector2 xz => new Vector2(_x, _z);
        public Vector2 yz => new Vector2(_y, _z);
        
        public Vector2 yx => new Vector2(_y, _x);
        public Vector2 zx => new Vector2(_z, _x);
        public Vector2 zy => new Vector2(_z, _y);

        public Vector3 xzy => new Vector3(_x, _z, _y);

        public Vector3 zxy => new Vector3(_z, _x, _y);
        public Vector3 zyx => new Vector3(_z, _y, _x);

        public Vector3 yxz => new Vector3(_y, _x, _z);
        public Vector3 yzx => new Vector3(_y, _z, _x);

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

        public Vector3 Cross(Vector3 v) => new Vector3(y * v.z - z * v.y,
                                                       z * v.x - x * v.z,
                                                       x * v.y - y * v.x);

        public static Vector3 Cross(Vector3 v1, Vector3 v2) => v1.Cross(v2);

        public static float Length(Vector3 v) => (float)Math.Sqrt(LengthSquared(v));

        public static float LengthSquared(Vector3 v) => (v.x * v.x) + (v.y * v.y) + (v.z * v.z);

        public static Vector3 Normalized(Vector3 v) => v / Length(v);

        public Vector3 Transform(Matrix4x4 m)
        {
            return new Vector3(
                x * m[0, 0] + y * m[1, 0] + z * m[2, 0] + m[3, 0],
                x * m[0, 1] + y * m[1, 1] + z * m[2, 1] + m[3, 1],
                x * m[0, 2] + y * m[1, 2] + z * m[2, 2] + m[3, 2]
                );
        }

        public Vector3 Transform(Quaternion q)
        {
            Quaternion c = q.normalized;
            Quaternion r = (c * this) * c.conjugate;

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
        public override string ToString()
        {
            return $"({x},{y},{z})";
        }
        public static float[] ToArray(Vector3[] values)
        {
            List<float> resList = new List<float>();
            foreach (Vector3 v in values)
                resList.AddRange(v.ToArray());

            return resList.ToArray();
        }

    }
}
