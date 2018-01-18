using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths
{
    public struct Quaternion
    {
        float _x, _y, _z, _w;
        
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

        public float w
        {
            get => _w;
            set => _w = value;
        }

        public static Quaternion Identity => new Quaternion(0,0,0,1);

        public Quaternion conjugate => Conjugate(this);

        public float length => Length(this);
        public float lengthSquard => LengthSquared(this);
        public Quaternion normalized => Normalized(this);

        public Quaternion(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public void SetIdentity()
        {
            _x = 0;
            _y = 0;
            _z = 0;
            _w = 1;
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
                return Quaternion.CreateFromAxisAngle(axis, 180);
            }
            axis = Vector3.Cross(start, target);
            float s = (float)Math.Sqrt((1f + cosTheta) * 2f);
            float invs = 1f / s;

            return new Quaternion(axis.x * invs, axis.y * invs, axis.z * invs, s * 0.5f);
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

        public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
        {
            Vector3 a = axis.normalized;

            float hSA = (float)Math.Sin((angle * Constants.ToRadians) * 0.5d);
            float hCA = (float)Math.Cos((angle * Constants.ToRadians) * 0.5d);

            return new Quaternion(a.x * hSA, a.y * hSA, a.z * hSA, hCA);
        }

        public Matrix4x4 ToRotationMatrix() => Matrix4x4.CreateRotationMatrix(this);

        public static float Length(Quaternion q) => (float)Math.Sqrt(LengthSquared(q));
        public static float LengthSquared(Quaternion q) => q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;

        public static Quaternion Normalized(Quaternion q)
        {
            float l = Length(q);
            return new Quaternion(q.x / l, q.y / l, q.z / l, q.w / l);
        }

        public static Quaternion Conjugate(Quaternion q) => new Quaternion(-q.x, -q.y, -q.z, q.w);

        public static Quaternion operator * (Quaternion q1, Quaternion q2)
        {
            return new Quaternion(
                q1.x * q2.w + q1.w * q2.x + q1.y * q2.z + q1.z * q2.y,
                q1.y * q2.w + q1.w * q2.y + q1.z * q2.x + q1.x * q2.z,
                q1.z * q2.w + q1.w * q2.z + q1.x * q2.y + q1.y * q2.x,
                q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z);
        }

        public static Quaternion operator * (Quaternion q, Vector3 v)
        {
            return new Quaternion(
                q.w * v.x + q.y * v.z - q.z * v.y,
                q.w * v.y + q.z * v.x - q.x * v.z,
                q.w * v.z + q.x * v.y - q.y * v.x,
                -q.x * v.x - q.y * v.y - q.z * v.z
                );
        }
        
    }
}
