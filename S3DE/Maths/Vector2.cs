using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths
{
    
    public struct S3DE_Vector2
    {
        //Needs to be represented as a Vector3 internally otherwise things don't work.
        //Don't ask why, just accept it.
        System.Numerics.Vector3 vec;

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

        public static S3DE_Vector2 Zero => new S3DE_Vector2(0, 0);
        public static S3DE_Vector2 One => new S3DE_Vector2(1, 1);
        public float[] ToArray() => new float[] {X, Y};

        public S3DE_Vector2(float f)
        {
            vec = new System.Numerics.Vector3(f,f,0);
        }

        public S3DE_Vector2(float x, float y)
        {
            vec = new System.Numerics.Vector3(x, y,0);
        }

        public float Sum() => X + Y;

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public static S3DE_Vector2 operator -(S3DE_Vector2 v1, S3DE_Vector2 v2) { System.Numerics.Vector3 v = (v1.vec - v2.vec); return new S3DE_Vector2(v.X, v.Y); }
        public static S3DE_Vector2 operator +(S3DE_Vector2 v1, S3DE_Vector2 v2) { System.Numerics.Vector3 v = (v1.vec + v2.vec); return new S3DE_Vector2(v.X, v.Y); }
        public static S3DE_Vector2 operator /(S3DE_Vector2 v1, S3DE_Vector2 v2) { System.Numerics.Vector3 v = (v1.vec / v2.vec); return new S3DE_Vector2(v.X, v.Y); }
        public static S3DE_Vector2 operator *(S3DE_Vector2 v1, S3DE_Vector2 v2) { System.Numerics.Vector3 v = (v1.vec * v2.vec); return new S3DE_Vector2(v.X, v.Y); }

        public static S3DE_Vector2 operator *(S3DE_Vector2 v1, float f) { System.Numerics.Vector3 v = (v1.vec * f); return new S3DE_Vector2(v.X, v.Y); }
        public static bool operator == (S3DE_Vector2 v1, S3DE_Vector2 v2)
        {
            if (v1.vec == v2.vec)
                return true;

            return false;
        }

        public static bool operator !=(S3DE_Vector2 v1, S3DE_Vector2 v2) => !(v1 == v2);
    }
}
