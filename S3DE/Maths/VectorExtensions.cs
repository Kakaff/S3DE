using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace S3DE.Maths
{

    public static class VectorExtensions
    {
        #region Matrix4x4

        #endregion

        #region Quaternion
        public static Quaternion Quat_CreateLookAt(Vector3 position, Vector3 target, Vector3 up)
        {
            Vector3 dir = Vec3_DirectionTo(position, target);

            Quaternion q1 = Quat_RotationBetweenVectors(Vec3_Forward, dir);
            Vector3 right = Vector3.Cross(dir, up);

            Vector3 newUp = Vector3.Transform(Vec3_Up,q1);
            Quaternion q2 = Quat_RotationBetweenVectors(newUp, Vector3.Cross(right, dir));
            return q2 * q1;
        }

        public static Quaternion Quat_RotationBetweenVectors(Vector3 start, Vector3 target)
        {
            float cosTheta = Vector3.Dot(start, target);
            Vector3 axis;

            if (cosTheta < -1f + 0.00001f)
            {
                axis = Vector3.Cross(Vec3_Forward, start);
                if (axis.LengthSquared() < 0.0001f)
                    axis = Vector3.Cross(Vec3_Right, start);

                axis = axis.Normalized();
                return Quat_CreateFromAxisAngle(axis, 180f);
            }

            axis = Vector3.Cross(start, target);
            float s = (float)Math.Sqrt((1f + cosTheta) * 2f);
            float invs = 1f / s;

            return new Quaternion(axis * invs, s * 0.5f);
        }

        public static Quaternion Quat_CreateFromAxisAngle(Vector3 axis, float angle)
        {
            Vector3 a = axis.Normalized();
            double d = (angle * Constants.ToRadians) * 0.5d;

            float hSA = (float)Math.Sin(d);
            float hCA = (float)Math.Cos(d);

            return new Quaternion(a * hSA, hCA);
        }

        public static Vector4 ToVector4(this Quaternion q) => new Vector4(q.X, q.Y, q.Z, q.W);
        public static Vector3 XYZ(this Quaternion q) => new Vector3(q.X, q.Y, q.Z);
        public static Quaternion conjugate(this Quaternion q) => Quaternion.Conjugate(q);
        public static Matrix4x4 ToRotationMatrix(this Quaternion q) => Matrix4x4.CreateRotationMatrix(q);

        #endregion

        #region Vector4
        public static Vector4 Vec4_Zero => new Vector4(0, 0, 0, 0);
        public static Vector3 XYZ(this Vector4 v) => new Vector3(v.X, v.Y, v.Z);
        public static Quaternion ToQuaternion(this Vector4 v) => new Quaternion(v.X, v.Y, v.Z, v.W);
        public static float Sum(this Vector4 v) => (new System.Numerics.Vector2(v.X, v.Y) + new System.Numerics.Vector2(v.Z, v.W)).Sum();
        #endregion

        #region Vector3
        public static Vector4 ToVector4(this Vector3 v) => new Vector4(v.X, v.Y, v.Z, 0);
        public static S3DE_Vector2 XY(this Vector3 v) => new S3DE_Vector2(v.X, v.Y);
        public static S3DE_Vector2 XZ(this Vector3 v) => new S3DE_Vector2(v.X, v.Z);
        public static S3DE_Vector2 YZ(this Vector3 v) => new S3DE_Vector2(v.Y, v.Z);

        public static S3DE_Vector2 YX(this Vector3 v) => new S3DE_Vector2(v.Y, v.X);
        public static S3DE_Vector2 ZX(this Vector3 v) => new S3DE_Vector2(v.Z, v.X);
        public static S3DE_Vector2 ZY(this Vector3 v) => new S3DE_Vector2(v.Z, v.Y);

        public static Vector3 XZY(this Vector3 v) => new Vector3(v.X, v.Z, v.Y);

        public static Vector3 ZXY(this Vector3 v) => new Vector3(v.Z, v.X, v.Y);
        public static Vector3 ZYX(this Vector3 v) => new Vector3(v.Z, v.Y, v.X);

        public static Vector3 YXZ(this Vector3 v) => new Vector3(v.Y, v.X, v.Z);
        public static Vector3 YZX(this Vector3 v) => new Vector3(v.Y, v.Z, v.X);

        public static float[] ToArray(this Vector3 v) => new float[] {v.X, v.Y, v.Z };

        public static Vector3 Vec3_Zero => new Vector3(0, 0, 0);
        public static Vector3 Vec3_One => new Vector3(1, 1, 1);

        public static Vector3 Vec3_Right => new Vector3(1, 0, 0);
        public static Vector3 Vec3_Left => new Vector3(-1,0,0);

        public static Vector3 Vec3_Up => new Vector3(0, 1, 0);
        public static Vector3 Vec3_Down => new Vector3(0,-1,0);

        public static Vector3 Vec3_Forward => new Vector3(0, 0, 1);
        public static Vector3 Vec3_Backward => new Vector3(0,0,-1);

        public static Vector3 Transform(this Vector3 v,Matrix4x4 m)
        {
            return new Vector3(v.X) * new Vector3(m[0, 0], m[0, 1], m[0, 2]) +
             new Vector3(v.Y) * new Vector3(m[1, 0], m[1, 1], m[1, 2]) +
             new Vector3(v.Z) * new Vector3(m[2, 0], m[2, 1], m[2, 2]) +
             new Vector3(m[3, 0], m[3, 1], m[3, 2]);
        }

        public static Vector3 Normalized(this Vector3 v) => Vector3.Normalize(v);

        public static Vector3 Sign(this Vector3 v) => new Vector3(Math.Sign(v.X), Math.Sign(v.Y), Math.Sign(v.Z));

        public static Vector3 Vec3_Sign(Vector3 v) => new Vector3(Math.Sign(v.X), Math.Sign(v.Y), Math.Sign(v.Z));
        public static Vector3 Vec3_DirectionTo(Vector3 start, Vector3 target) => (target - start).Normalized();
        public static Vector3 Cross(this Vector3 v1, Vector3 v2) => Vector3.Cross(v1, v2);

        #endregion

        #region Vector2
        public static float Sum(this System.Numerics.Vector2 v) => v.X + v.Y;
        #endregion
    }
}
