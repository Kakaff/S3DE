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
            SetIdentity();
        }

        public Matrix4x4 SetIdentity()
        {
            
            mV[0] = new float[] { 1, 0, 0, 0 };
            mV[1] = new float[] { 0, 1, 0, 0 };
            mV[2] = new float[] { 0, 0, 1, 0 };
            mV[3] = new float[] { 0, 0, 0, 1 };
            
            return this;
        }

        public static Matrix4x4 CreateProjectionMatrix_FoV(float fov, float zNear, float zFar, float aspect)
        {
            Matrix4x4 m = new Matrix4x4();

            Vector2 v2_0 = new Vector2(zNear, -zNear) - new Vector2(zFar);
            Vector2 v2_1 = new Vector2((float)Constants.ToRadians, 2f) * new Vector2((fov / 2f), zFar);
            v2_1.X = (float)Math.Tan(v2_1.X);

            float t = v2_1.X;

            v2_1 *= new Vector2(aspect, zNear);
            Vector4 v4_0 = new Vector4(1, 1, (v2_0.Y), v2_1.Y) / new Vector4(v2_1.X, t, v2_0.X, v2_0.X);

            m[0, 0] = v4_0.X;
            m[1, 1] = v4_0.Y;
            m[2, 2] = v4_0.Z;
            m[3, 2] = v4_0.W;
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
            m[3, 0] = translation.X;
            m[1, 1] = 1;
            m[3, 1] = translation.Y;
            m[2, 2] = 1;
            m[3, 2] = translation.Z;
            m[3, 3] = 1;

            return m;
        }

        public static Matrix4x4 CreateScaleMatrix(Vector3 scale)
        {
            Matrix4x4 m = new Matrix4x4();
            m[0, 0] = scale.X;
            m[1, 1] = scale.Y;
            m[2, 2] = scale.Z;
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

            Vector4 sqv = quat.ToVector4() * quat.ToVector4();
            Vector3 xv = new Vector3(quat.X,quat.X,quat.Y) * new Vector3(quat.Y,quat.Z,quat.Z);
            Vector3 wv = quat.XYZ * new Vector3(quat.W);
            float invs = 1 / (new Vector2(sqv.X, sqv.Y) + new Vector2(sqv.Z, sqv.W)).Sum();

            Vector3 v1_0 = new Vector3(sqv.X, -sqv.X, -sqv.X) +
            new Vector3(-sqv.Y, sqv.Y, -sqv.Y) +
            new Vector3(-sqv.Z, -sqv.Z, sqv.Z) +
            new Vector3(sqv.W) *
            invs;


            Vector2 v2_0 = new Vector2(2) * 
                (new Vector2(xv.Z) + 
                new Vector2(-wv.X, wv.X)) *
                invs;

            Vector4 v4_0 = new Vector4(2) *
                (new Vector4(xv.X, xv.X, xv.Y, xv.Y) + 
                new Vector4(-wv.Z, wv.Z, wv.Y, -wv.Y)) *
                invs;

            m[0, 0] = v1_0.X;
            m[1, 1] = v1_0.Y;
            m[2, 2] = v1_0.Z;

            m[1, 0] = v4_0.X;
            m[0, 1] = v4_0.Y;
            m[2, 0] = v4_0.Z;
            m[0, 2] = v4_0.W;
            
            m[2, 1] = v2_0.X;
            m[1, 2] = v2_0.Y;

            m[3, 3] = 1;

            return m;
        }

        public static Matrix4x4 operator * (Matrix4x4 m1, Matrix4x4 m2)
        {
            Matrix4x4 m = new Matrix4x4();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    m[i, j] = 
                        (new Vector4(m1[i,0],m1[i,1],m1[i,2],m1[i,3]) * 
                        new Vector4(m2[0,j],m2[1,j],m2[2,j],m2[3,j]))
                        .Sum();
                }

            return m;
        }

        public static Matrix4x4 operator *(Matrix4x4 m, Quaternion q) => m * q.conjugate.ToRotationMatrix();

        public float[] ToArray()
        {
            List<float> values = new List<float>();
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    values.Add(mV[x][y]);

            return values.ToArray();
        }

        public byte[] ToByteArray()
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    bytes.AddRange(BitConverter.GetBytes(mV[i][j]));

            return bytes.ToArray();
        }

        public static byte[] ToByteArray(params Matrix4x4[] matrices)
        {
            List<byte> res = new List<byte>();
            foreach (Matrix4x4 m in matrices)
                res.AddRange(m.ToByteArray());
            
            return res.ToArray();
        }
    }
}
