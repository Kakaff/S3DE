using S3DE.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths
{
    public sealed class Matrix4x4
    {
        float[][] mV;

        public float this[int x, int y]
        {
            get => mV[x][y];
            set => mV[x][y] = value;
        }


        public Matrix4x4()
        {
            mV = new float[4][];
            mV[0] = new float[4];
            mV[1] = new float[4];
            mV[2] = new float[4];
            mV[3] = new float[4];
        }

        public Matrix4x4 SetIdentity()
        {
            mV[0][0] = 1; mV[0][1] = 0; mV[0][2] = 0; mV[0][3] = 0;
            mV[1][0] = 0; mV[1][1] = 1; mV[1][2] = 0; mV[1][3] = 0;
            mV[2][0] = 0; mV[2][1] = 0; mV[2][2] = 1; mV[2][3] = 0;
            mV[3][0] = 0; mV[3][1] = 0; mV[3][2] = 0; mV[3][3] = 1;
            
            return this;
        }

        public static Matrix4x4 CreateProjectionMatrix_FoV(float fov, float zNear, float zFar, float aspect)
        {
            Matrix4x4 m = new Matrix4x4();

            S3DE_Vector2 v2_0 = new S3DE_Vector2(zNear, -zNear) - new S3DE_Vector2(zFar);
            S3DE_Vector2 v2_1 = new S3DE_Vector2((float)Constants.ToRadians, 2f) * new S3DE_Vector2((fov / 2f), zFar);
            v2_1.X = (float)Math.Tan(v2_1.X);

            float t = v2_1.X;

            v2_1 *= new S3DE_Vector2(aspect, zNear);
            System.Numerics.Vector4 v4_0 = new System.Numerics.Vector4(1, 1, (v2_0.Y), v2_1.Y) / new System.Numerics.Vector4(v2_1.X, t, v2_0.X, v2_0.X);

            m.mV[0][0] = v4_0.X;
            m.mV[1][1] = v4_0.Y;
            m.mV[2][2] = v4_0.Z;
            m.mV[3][2] = v4_0.W;
            m.mV[2][3] = 1f;
            m.mV[3][3] = 0;
            return m;
        }

        public static Matrix4x4 CreateViewMatrix(System.Numerics.Vector3 cameraPosition, System.Numerics.Vector3 cameraForward, System.Numerics.Vector3 cameraUp)
        {
            Matrix4x4 m = new Matrix4x4();
            Matrix4x4 tm = CreateTranslationMatrix(-cameraPosition);
            Matrix4x4 rm = VectorExtensions.Quat_CreateLookAt(VectorExtensions.Vec3_Zero, cameraForward, cameraUp).conjugate().ToRotationMatrix();

            return tm * rm;
        }


        public static Matrix4x4 CreateLookAtMatrix(System.Numerics.Vector3 position, System.Numerics.Vector3 target, System.Numerics.Vector3 up) =>
            VectorExtensions.Quat_CreateLookAt(position, target, up).ToRotationMatrix();

        public static Matrix4x4 CreateTransformMatrix(System.Numerics.Vector3 translation, System.Numerics.Vector3 scale, System.Numerics.Quaternion rotation) =>
            CreateScaleMatrix(scale) * CreateRotationMatrix(rotation) * CreateTranslationMatrix(translation);


        public static Matrix4x4 CreateTranslationMatrix(System.Numerics.Vector3 translation)
        {
            Matrix4x4 m = new Matrix4x4();
            m.mV[0][0] = 1;
            m.mV[3][0] = translation.X;
            m.mV[1][1] = 1;
            m.mV[3][1] = translation.Y;
            m.mV[2][2] = 1;
            m.mV[3][2] = translation.Z;
            m.mV[3][3] = 1;

            return m;
        }

        public static Matrix4x4 CreateScaleMatrix(System.Numerics.Vector3 scale)
        {
            Matrix4x4 m = new Matrix4x4();
            m.mV[0][0] = scale.X;
            m.mV[1][1] = scale.Y;
            m.mV[2][2] = scale.Z;
            m.mV[3][3] = 1;

            return m;
        }

        public static Matrix4x4 Transpose(Matrix4x4 m)
        {
            return null;
        }

        public static Matrix4x4 CreateRotationMatrix(System.Numerics.Quaternion quat)
        {
            Matrix4x4 m = new Matrix4x4();

            System.Numerics.Vector4 sqv = quat.ToVector4() * quat.ToVector4();
            System.Numerics.Vector3 xv = new System.Numerics.Vector3(quat.X, quat.X, quat.Y) * new System.Numerics.Vector3(quat.Y, quat.Z, quat.Z);
            System.Numerics.Vector3 wv = quat.XYZ() * quat.W;
            float invs = 1 / (new S3DE_Vector2(sqv.X, sqv.Y) + new S3DE_Vector2(sqv.Z, sqv.W)).Sum();

            System.Numerics.Vector3 v1_0 = new System.Numerics.Vector3(sqv.X, -sqv.X, -sqv.X) +
            new System.Numerics.Vector3(-sqv.Y, sqv.Y, -sqv.Y) +
            new System.Numerics.Vector3(-sqv.Z, -sqv.Z, sqv.Z) +
            new System.Numerics.Vector3(sqv.W) *
            invs;
            
            S3DE_Vector2 v2_0 = new S3DE_Vector2(2) *
                (new S3DE_Vector2(xv.Z) +
                new S3DE_Vector2(-wv.X, wv.X)) *
                invs;

            System.Numerics.Vector4 v4_0 = new System.Numerics.Vector4(2) *
                (new System.Numerics.Vector4(xv.X, xv.X, xv.Y, xv.Y) +
                new System.Numerics.Vector4(-wv.Z, wv.Z, wv.Y, -wv.Y)) *
                invs;

            m.mV[0][0] = v1_0.X;
            m.mV[1][1] = v1_0.Y;
            m.mV[2][2] = v1_0.Z;

            m.mV[1][0] = v4_0.X;
            m.mV[0][1] = v4_0.Y;
            m.mV[2][0] = v4_0.Z;
            m.mV[0][2] = v4_0.W;

            m.mV[2][1] = v2_0.X;
            m.mV[1][2] = v2_0.Y;

            m.mV[3][3] = 1;

            return m;
        }

        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2)
        {
            Matrix4x4 m = new Matrix4x4();
            
            System.Numerics.Vector4 v4_0, rV4_0, rV4_1, rV4_2, rV4_3;
            for (int i = 0; i < 4; i++) {

                rV4_0 = (new System.Numerics.Vector4(m1.mV[i][0], m1.mV[i][1], m1.mV[i][2], m1.mV[i][3]) *
                        new System.Numerics.Vector4(m2.mV[0][0], m2.mV[1][0], m2.mV[2][0], m2.mV[3][0]));
                rV4_1 =
                        (new System.Numerics.Vector4(m1.mV[i][0], m1.mV[i][1], m1.mV[i][2], m1.mV[i][3]) *
                        new System.Numerics.Vector4(m2.mV[0][1], m2.mV[1][1], m2.mV[2][1], m2.mV[3][1]));
                rV4_2 =
                        (new System.Numerics.Vector4(m1.mV[i][0], m1.mV[i][1], m1.mV[i][2], m1.mV[i][3]) *
                        new System.Numerics.Vector4(m2.mV[0][2], m2.mV[1][2], m2.mV[2][2], m2.mV[3][2]));
                rV4_3 =
                        (new System.Numerics.Vector4(m1.mV[i][0], m1.mV[i][1], m1.mV[i][2], m1.mV[i][3]) *
                        new System.Numerics.Vector4(m2.mV[0][3], m2.mV[1][3], m2.mV[2][3], m2.mV[3][3]));

                v4_0 = new System.Numerics.Vector4(rV4_0.X, rV4_1.X, rV4_2.X, rV4_3.X) +
                        new System.Numerics.Vector4(rV4_0.Y, rV4_1.Y, rV4_2.Y, rV4_3.Y) +
                        new System.Numerics.Vector4(rV4_0.Z, rV4_1.Z, rV4_2.Z, rV4_3.Z) +
                        new System.Numerics.Vector4(rV4_0.W, rV4_1.W, rV4_2.W, rV4_3.W);

                m.mV[i][0] = v4_0.X;
                m.mV[i][1] = v4_0.Y;
                m.mV[i][2] = v4_0.Z;
                m.mV[i][3] = v4_0.W;

            }
            return m;
        }

        public static Matrix4x4 operator *(Matrix4x4 m, System.Numerics.Quaternion q) => m * q.conjugate().ToRotationMatrix();

        public float[] ToArray()
        {
            float[] values = new float[16];

            for (int i = 0; i < 4; i++)
                Buffer.BlockCopy(mV[i], 0, values, i * 16, 16);

            return values;
        }

        public byte[] ToByteBuffer()
        {
            byte[] buff = new byte[64];
            Buffer.BlockCopy(mV[0], 0, buff, 0, 16);
            Buffer.BlockCopy(mV[1], 0, buff, 16, 16);
            Buffer.BlockCopy(mV[2], 0, buff, 32, 16);
            Buffer.BlockCopy(mV[3], 0, buff, 48, 16);

            return buff;
        }

        public static ByteBuffer ToByteBuffer(params Matrix4x4[] matrices)
        {
            
            ByteBuffer buff = ByteBuffer.Create(64 * matrices.Length);
            ByteBuffer b;
            foreach (Matrix4x4 m in matrices)
            {
                buff.AddRange(m.ToByteBuffer());
            }
            
            return buff;
        }
    }
}
