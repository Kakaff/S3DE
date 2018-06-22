using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths
{
    //SIMD Enabled Vector3
    
    public struct Vector3
    {
        System.Numerics.Vector3 internalVector;

        public System.Numerics.Vector3 InternalVector => internalVector;

        public float X
        {
            get => internalVector.X;
            set => internalVector.Y = value;
        }

        public float Y
        {
            get => internalVector.Y;
            set => internalVector.Y = value;
        }

        public float Z
        {
            get => internalVector.Z;
            set => internalVector.Z = value;
        }

        public Vector2 XY => new Vector2(X, Y);
        public Vector2 XZ => new Vector2(X, Z);
        public Vector2 YZ => new Vector2(Y, Z);
        
        public Vector2 YX => new Vector2(Y, X);
        public Vector2 ZX => new Vector2(Z, X);
        public Vector2 ZY => new Vector2(Z, Y);

        public Vector3 XZY => new Vector3(X, Z, Y);

        public Vector3 ZXY => new Vector3(Z, X, Y);
        public Vector3 ZYX => new Vector3(Z, Y, X);

        public Vector3 YXZ => new Vector3(Y, X, Z);
        public Vector3 YZX => new Vector3(Y, Z, X);

        public float[] ToArray() => new float[] {X,Y,Z};

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
            internalVector = new System.Numerics.Vector3(x, y, z);
        }

        public Vector3(float f)
        {
            internalVector = new System.Numerics.Vector3(f);
        }

        public Vector3(System.Numerics.Vector3 vec)
        {
            this.internalVector = vec;
        }

        public float Sum => X + Y + Z;

        public static float Dot(Vector3 v1, Vector3 v2) => System.Numerics.Vector3.Dot(v1.internalVector, v2.internalVector);

        public Vector3 Cross(Vector3 v) => new Vector3(System.Numerics.Vector3.Cross(internalVector, v.internalVector));

        public static Vector3 Cross(Vector3 v1, Vector3 v2) => v1.Cross(v2);

        public static float Length(Vector3 v) => v.internalVector.Length();

        public static float LengthSquared(Vector3 v) => v.internalVector.LengthSquared();

        public static Vector3 Normalized(Vector3 v) => v / Length(v);

        public Vector3 Transform(Matrix4x4 m)
        {
           return new Vector3(X) * new Vector3(m[0, 0], m[0, 1], m[0, 2]) +
            new Vector3(Y) * new Vector3(m[1, 0], m[1, 1], m[1, 2]) + 
            new Vector3(Z) * new Vector3(m[2, 0], m[2, 1], m[2, 2]) + 
            new Vector3(m[3, 0], m[3, 1], m[3, 2]);
        }

        public Vector3 Transform(Quaternion q)
        {
            return new Vector3(System.Numerics.Vector3.Transform(internalVector, q.InternalQuaternion));
        }

        public static Vector3 DirectionTo(Vector3 start, Vector3 target) => (target - start).normalized;

        public static Vector3 Clamp(Vector3 v, float min,float max)
        {
            return new Vector3(System.Numerics.Vector3.Clamp(v.internalVector, new System.Numerics.Vector3(min), new System.Numerics.Vector3(max)));
        }

        public static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max)
        {
            return new Vector3(System.Numerics.Vector3.Clamp(v.internalVector, min.internalVector, max.internalVector));
        }

        public static Vector3 Sign(Vector3 v)
        {
            return new Vector3(Math.Sign(v.X), Math.Sign(v.Y), Math.Sign(v.Z));
        }

        public static Vector3 operator * (Vector3 vec, float f)
        {
            return new Vector3(vec.internalVector * f);
        }

        public static Vector3 operator / (Vector3 vec, float f)
        {
            return new Vector3(vec.internalVector / f);
        }

        public static Vector3 operator * (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.internalVector * v2.internalVector);
        }

        public static Vector3 operator + (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.internalVector + v2.internalVector);
        }

        public static Vector3 operator - (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.internalVector - v2.internalVector);
        }

        public static bool operator == (Vector3 v1, Vector3 v2)
        {
            if (v1.internalVector == v2.internalVector)
                return true;
            else
                return false;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2) => !(v1 == v2);

        public static Vector3 operator -(Vector3 v) => new Vector3(-v.internalVector);

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
            return $"({X},{Y},{Z})";
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
