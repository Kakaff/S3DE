using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths
{
    public struct Vector4
    {
        System.Numerics.Vector4 vec;

        public float X
        {
            get => vec.X;
            set => vec.X = value;
        }

        public float Y
        {
            get => vec.Y;
            set => vec.Y = value;
        }

        public float Z
        {
            get => vec.Z;
            set => vec.Z = value;
        }

        public float W
        {
            get => vec.W;
            set => vec.W = value;
        }

        public Vector3 XYZ => new Vector3(X,Y,Z);

        public Vector4(float x, float y, float z, float w)
        {
            vec = new System.Numerics.Vector4(x, y, z, w);
        }

        public Vector4(float f)
        {
            vec = new System.Numerics.Vector4(f);
        }
        public Vector4(System.Numerics.Vector4 vec)
        {
            this.vec = vec;
        }

        public static Vector4 Max(Vector4 v1, Vector4 v2)
        {
            return new Vector4(System.Numerics.Vector4.Max(v1.vec, v2.vec));
        }

        public static Vector4 Sqrt(Vector4 v)
        {
            return new Vector4(System.Numerics.Vector4.SquareRoot(v.vec));
        }

        public float Sum() {
           return (new Vector2(X, Y) + new Vector2(Z, W)).Sum();
        }

        public static Vector4 operator /(Vector4 v, float f) => new Vector4(v.vec / f);
        public static Vector4 operator *(Vector4 v, float f) => new Vector4(v.vec * f);

        public static Vector4 operator *(Vector4 v1, Vector4 v2) => new Vector4(v1.vec * v2.vec);

        public static Vector4 operator /(Vector4 v1, Vector4 v2) => new Vector4(v1.vec / v2.vec);
        public static Vector4 operator -(Vector4 v1, Vector4 v2) => new Vector4(v1.vec - v2.vec);
        public static Vector4 operator +(Vector4 v1, Vector4 v2) => new Vector4(v1.vec + v2.vec);
        public static implicit operator Vector4(Vector3 v) => new Vector4(v.X,v.Y,v.Z,0);
        
        

        public Quaternion ToQuaternion() => new Quaternion(X, Y, Z, W);
        public float[] ToArray() => new float[] { X, Y, Z, W };
    }
}
