using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths
{
    public sealed class Matrix4x4
    {
        float[,] mV;

        public float this[int x, int y]
        {
            get => mV[x, y];
            set => mV[x, y] = value;
        }

        public Matrix4x4()
        {
            mV = new float[4, 4];
            SetIdentity();
        }

        public Matrix4x4 SetIdentity()
        {
            mV[0, 0] = 1;
            mV[1, 1] = 1;
            mV[2, 2] = 1;
            mV[3, 3] = 1;

            return this;
        }

        public static Matrix4x4 CreateProjectionMatrix_FoV(float fov, float zNear, float zFar, float aspect)
        {
            Matrix4x4 m = new Matrix4x4();

            float t = (float)Math.Tan(Constants.ToRadians * (fov / 2d));
            float r = zNear - zFar;

            m[0, 0] = 1f / (t * aspect);
            m[1, 1] = 1f / t;
            m[2, 2] = (-zNear - zFar) / r;
            m[3, 2] = 2f * zFar * zNear / r;
            m[2, 3] = 1f;
            m[3, 3] = 0;
            return m;
        }

        public static Matrix4x4 CreateViewMatrix(Vector3 cameraPosition, Vector3 cameraForward, Vector3 cameraUp)
        {
            Matrix4x4 m = new Matrix4x4();
            Matrix4x4 tm = CreateTranslationMatrix(-cameraPosition);
            Matrix4x4 rm = Quaternion.CreateLookAt(Vector3.Zero, cameraForward, cameraUp).conjugate.ToRotationMatrix();

            return tm * rm;
        }
        

        public static Matrix4x4 CreateLookAtMatrix(Vector3 position, Vector3 target, Vector3 up) =>
            Quaternion.CreateLookAt(position,target,up).ToRotationMatrix();

        public static Matrix4x4 CreateTransformMatrix(Vector3 translation, Vector3 scale, Quaternion rotation) =>
            CreateScaleMatrix(scale) * CreateRotationMatrix(rotation) * CreateTranslationMatrix(translation);
        

        public static Matrix4x4 CreateTranslationMatrix(Vector3 translation)
        {
            Matrix4x4 m = new Matrix4x4();
            m[0, 0] = 1;
            m[3, 0] = translation.x;
            m[1, 1] = 1;
            m[3, 1] = translation.y;
            m[2, 2] = 1;
            m[3, 2] = translation.z;
            m[3, 3] = 1;

            return m;
        }

        public static Matrix4x4 CreateScaleMatrix(Vector3 scale)
        {
            Matrix4x4 m = new Matrix4x4();
            m[0, 0] = scale.x;
            m[1, 1] = scale.y;
            m[2, 2] = scale.z;
            m[3, 3] = 1;

            return m;
        }

        public static Matrix4x4 Transpose(Matrix4x4 m)
        {
            return null;
        }

        public static Matrix4x4 CreateRotationMatrix(Quaternion quat)
        {
            
            Matrix4x4 m = new Matrix4x4();
            float sqw = quat.w * quat.w;
            float sqx = quat.x * quat.x;
            float sqy = quat.y * quat.y;
            float sqz = quat.z * quat.z;

            float xy = quat.x * quat.y;
            float zw = quat.z * quat.w;

            float xz = quat.x * quat.z;
            float yw = quat.y * quat.w;

            float yz = quat.y * quat.z;
            float xw = quat.x * quat.w;

            float invs = 1 / (sqx + sqy + sqz + sqw);

            m[0, 0] = (sqx - sqy - sqz + sqw) * invs;
            m[1, 1] = (-sqx + sqy - sqz + sqw) * invs;
            m[2, 2] = (-sqx - sqy + sqz + sqw) * invs;

            m[1, 0] = 2 * (xy - zw) * invs;
            m[0, 1] = 2 * (xy + zw) * invs;

            m[2, 0] = 2 * (xz + yw) * invs;
            m[0, 2] = 2 * (xz - yw) * invs;

            m[2, 1] = 2 * (yz - xw) * invs;
            m[1, 2] = 2 * (yz + xw) * invs;

            m[3, 3] = 1;

            return m;
        }

        public static Matrix4x4 operator * (Matrix4x4 m1, Matrix4x4 m2)
        {
            Matrix4x4 m = new Matrix4x4();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    m[i, j] =
                        m1[i, 0] * m2[0, j] +
                        m1[i, 1] * m2[1, j] +
                        m1[i, 2] * m2[2, j] +
                        m1[i, 3] * m2[3, j];

            return m;
        }

        public static Matrix4x4 operator *(Matrix4x4 m, Quaternion q) => m * q.conjugate.ToRotationMatrix();

        public float[] ToArray()
        {
            List<float> values = new List<float>();
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    values.Add(mV[x, y]);

            return values.ToArray();
        }
    }
}
