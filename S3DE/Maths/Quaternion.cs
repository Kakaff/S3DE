using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths
{

    //SIMD Quaternion
    /*
    public struct Quaternion
    {
        System.Numerics.Quaternion internalQuat;

        public System.Numerics.Quaternion InternalQuaternion => internalQuat;

        public float X
        {
            get => internalQuat.X;
            set => internalQuat.X = value;
        }

        public float Y
        {
            get => internalQuat.Y;
            set => internalQuat.Y = value;
        }

        public float Z
        {
            get => internalQuat.Z;
            set => internalQuat.Z = value;
        }

        public float W
        {
            get => internalQuat.W;
            set => internalQuat.W = value;
        }

        public static Quaternion Identity => new Quaternion(0,0,0,1);

        public Quaternion conjugate => Conjugate(this);

        public float length => Length(this);
        public float lengthSquard => LengthSquared(this);
        public Quaternion normalized => Normalized(this);

        public Quaternion(float x, float y, float z, float w)
        {
            internalQuat = new System.Numerics.Quaternion(x, y, z, w);
        }

        public Quaternion(Vector3 vec, float w)
        {
            internalQuat = new System.Numerics.Quaternion(vec.InternalVector, w);
        }
        public Quaternion(System.Numerics.Quaternion quat)
        {
            internalQuat = quat;
        }

        public void SetIdentity()
        {
            X = 0;
            Y = 0;
            Z = 0;
            W = 1;
        }

        public static Quaternion RotationBetweenVectors(Vector3 start, Vector3 target)
        {
            float cosTheta = Vector3.Dot(start, target);
            Vector3 axis;

            if (cosTheta < -1f + 0.00001f)
            {
                axis = Vector3.Cross(Vector3.Forward, start);
                if (axis.lengthSquared < 0.0001f)
                    axis = Vector3.Cross(Vector3.Right, start);

                axis = axis.normalized;
                return CreateFromAxisAngle(axis, 180f);
            }

            axis = Vector3.Cross(start, target);
            float s = (float)Math.Sqrt((1f + cosTheta) * 2f);
            float invs = 1f / s;

            return new Quaternion(axis * invs, s * 0.5f);
        }

        public static Quaternion CreateLookAt(Vector3 position, Vector3 target, Vector3 up)
        {
            Vector3 dir = Vector3.DirectionTo(position, target);
            
            Quaternion q1 = RotationBetweenVectors(Vector3.Forward, dir);
            Vector3 right = Vector3.Cross(dir, up);

            Vector3 newUp = Vector3.Up * q1;
            Quaternion q2 = RotationBetweenVectors(newUp, Vector3.Cross(right, dir));
            return q2 * q1;
        }

        public static Quaternion CreateLookAt(Vector3 position, Vector3 target)
        {
            return Identity;
        }

        public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
        {
            Vector3 a = axis.normalized;
            double d = (angle * Constants.ToRadians) * 0.5d;

            float hSA = (float)Math.Sin(d);
            float hCA = (float)Math.Cos(d);

            return new Quaternion(a * hSA, hCA);
        }

        public Vector3 XYZ => new Vector3(X, Y, Z);
        public Vector3 YZX => new Vector3(Y, Z, X);
        public Vector3 ZXY => new Vector3(Z, X, Y);
        public Vector2 YZ => new Vector2(Y, Z);

        public Vector4 ToVector4() => new Vector4(X, Y, Z, W);

        public Matrix4x4 ToRotationMatrix() => Matrix4x4.CreateRotationMatrix(this);

        public static float Length(Quaternion q) => q.internalQuat.Length();
        public static float LengthSquared(Quaternion q) => q.internalQuat.LengthSquared();

        public static Quaternion CreateFromRotationMatrix(Matrix4x4 m)
        {
            Vector4 v4_0 = new Vector4(1) +
            new Vector4(m[0, 0], -m[0, 0], -m[0, 0], m[0, 0]) +
            new Vector4(-m[1, 1], m[1, 1], -m[1, 1], m[1, 1]) +
            new Vector4(-m[2, 2], -m[2, 2], m[2, 2], m[2, 2]);

            v4_0 = Vector4.Max(new Vector4(0), v4_0);

            v4_0 = Vector4.Sqrt(v4_0);
            v4_0 /= 2;
            Vector3 v3_0 = v4_0.XYZ;
            
            v3_0 *= Vector3.Sign(v3_0 * 
                (new Vector3(m[2, 1], m[0, 2], m[1, 0]) - 
                new Vector3(m[1, 2], m[2, 0], m[0, 1])));
            
            return new Quaternion(v3_0,v4_0.W);
        }

        public static Quaternion Normalized(Quaternion q)
        {
            return new Quaternion(System.Numerics.Quaternion.Normalize(q.InternalQuaternion));
        }

        public static Quaternion Conjugate(Quaternion q) => new Quaternion(-q.X, -q.Y, -q.Z, q.W);
        
        public static Quaternion operator * (Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1.internalQuat * q2.internalQuat);
        }

        public static Quaternion operator * (Quaternion q, Vector3 v)
        {
            float w;
            Vector3 vecPart = new Vector3(q.W) * v + q.YZX * v.ZXY - q.ZXY * v.YZX;
            w = -q.X * v.X - q.Y * v.Y - q.Z * v.Z;
            
            return new Quaternion(vecPart, w);
        }

        public override string ToString() => $"(x:{X}|y:{Y}|z:{Z}|w:{W})";

    }
    */
}
