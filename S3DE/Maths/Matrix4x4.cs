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

        public static Matrix4x4 CreateRotationMatrix(Quaternion quat)
        {
            Matrix4x4 m = new Matrix4x4();
            Quaternion q = quat.normalized;

            float xx = 2.0f * (q.x * q.x);
            float yy = 2.0f * (q.y * q.y);
            float zz = 2.0f * (q.z * q.z);

            float xy = 2.0f * (q.x * q.y);
            float xz = 2.0f * (q.x * q.z);
            float xw = 2.0f * (q.x * q.w);

            float yz = 2.0f * (q.y * q.z);
            float yw = 2.0f * (q.y * q.w);

            float zw = 2.0f * (q.z * q.w);

            m[0, 0] = 1.0f - yy - zz;
            m[1, 0] = xy - zw;
            m[2, 0] = xz + yw;

            m[0, 1] = xy + zw;
            m[1, 1] = 1f - xx - zz;
            m[2, 1] = yz - xw;

            m[0, 2] = xz - yw;
            m[1, 2] = yz + xw;
            m[2, 2] = 1.0f - xx - yy;

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

        public static Matrix4x4 operator *(Matrix4x4 m, Quaternion q) => m * q.ToRotationMatrix();

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
