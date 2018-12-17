using S3DE.Maths.SIMD;
using System.Runtime.InteropServices;

namespace S3DE.Maths
{
    [StructLayout(LayoutKind.Sequential,Pack = 16)]
    public struct Vector4
    {
        public float x, y, z, w;
        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4(float f)
        {
            x = f;
            y = f;
            z = f;
            w = f;
        }

        public static unsafe Vector4 operator * (Vector4 v1, Vector4 v2)
        {
            Vector4 r = new Vector4();
            Vec128.Mul(&v1.x, &v2.x,&r.x);
            return r;
        }

        public static unsafe Vector4 operator * (Vector4 v1, float f)
        {
            return v1 * new Vector4(f);
        }

        public static unsafe Vector4 operator + (Vector4 v1, Vector4 v2)
        {
            Vector4 r = new Vector4();
            Vec128.Add(&v1.x, &v2.x, &r.x);
            return r;
        }

        public static unsafe Vector4 operator -(Vector4 v1, Vector4 v2)
        {
            Vector4 r = new Vector4();
            Vec128.Sub(&v1.x, &v2.x, &r.x);
            return r;
        }


        public static Vector4 operator / (Vector4 v, float i)
        {
            return new Vector4(v.x / i, v.y / i, v.z / i, v.w / i);
        }

        public Quaternion ToQuaternion()
        {
            return new Quaternion(x, y, z, w);
        }
    }
}
