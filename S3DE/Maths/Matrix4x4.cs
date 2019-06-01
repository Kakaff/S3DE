using System;
using System.Runtime.InteropServices;
using S3DE.Maths.SIMD;

namespace S3DE.Maths
{
    /*
     * Based on System.Numerics.Matrix4X4
     * With some minor changes/tweaks here and there.
     * .Net source found at:
     * https://github.com/dotnet/corefx/blob/master/src/System.Numerics.Vectors/src/System/Numerics/Matrix4x4.cs
     */

    [StructLayout(LayoutKind.Sequential,Pack = 32)]
    public struct Matrix4x4
    {
        public float m00, m10, m20, m30,
                     m01, m11, m21, m31,
                     m02, m12, m22, m32,
                     m03, m13, m23, m33;


        public Matrix4x4 SetIdentity()
        {
            m00 = 1; m10 = 0; m20 = 0; m30 = 0;
            m01 = 0; m11 = 1; m21 = 0; m31 = 0;
            m02 = 0; m12 = 0; m22 = 1; m32 = 0;
            m03 = 0; m13 = 0; m23 = 0; m33 = 1;

            return this;
        }

        public static Matrix4x4 CreateScaleMatrix(Vector3 scale)
        {
            Matrix4x4 m = new Matrix4x4();

            m.m00 = scale.x;
            m.m11 = scale.y;
            m.m22 = scale.z;
            m.m33 = 1.0f;

            return m;
        }

        public static Matrix4x4 CreateTranslationMatrix(Vector3 v)
        {
            Matrix4x4 m = new Matrix4x4();

            m.m00 = 1.0f;
            m.m11 = 1.0f;
            m.m22 = 1.0f;
            
            m.m30 = v.x;
            m.m31 = v.y;
            m.m32 = v.z;
            m.m33 = 1.0f;

            return m;
        }

        public static Matrix4x4 CreateRotationMatrix(Quaternion quaternion)
        {
            if (SIMD_Math.IsEnabled)
                return SIMD_Math.QuaternionToRotationMatrix(quaternion);
            else
            {
                Matrix4x4 result = new Matrix4x4();
                float xx = quaternion.x * quaternion.x;
                float yy = quaternion.y * quaternion.y;
                float zz = quaternion.z * quaternion.z;

                float xy = quaternion.x * quaternion.y;
                float wz = quaternion.z * quaternion.w;
                float zx = quaternion.z * quaternion.x;
                float wy = quaternion.y * quaternion.w;
                float yz = quaternion.y * quaternion.z;
                float wx = quaternion.x * quaternion.w;

                result.m00 = 1.0f - 2.0f * (yy + zz);
                result.m10 = 2.0f * (xy - wz);
                result.m21 = 2.0f * (yz - wx);
                result.m02 = 2.0f * (zx - wy);
                result.m11 = 1.0f - 2.0f * (zz + xx);
                result.m01 = 2.0f * (xy + wz);
                result.m12 = 2.0f * (yz + wx);
                result.m20 = 2.0f * (zx + wy);
                result.m22 = 1.0f - 2.0f * (yy + xx);
                result.m33 = 1.0f;

                return result;
            }
        }

        public static Matrix4x4 CreateLookAt(Vector3 pos, Vector3 target, Vector3 up)
        {
            Vector3 zaxis = Vector3.Normalize(pos - target);
            Vector3 xaxis = Vector3.Normalize(Vector3.Cross(up, zaxis));
            Vector3 yaxis = Vector3.Cross(zaxis, xaxis);

            Matrix4x4 m = new Matrix4x4();

            m.m00 = xaxis.x;
            m.m01 = yaxis.x;
            m.m02 = zaxis.x;
            m.m10 = xaxis.y;
            m.m11 = yaxis.y;
            m.m12 = zaxis.y;
            m.m20 = xaxis.z;
            m.m21 = yaxis.z;
            m.m22 = zaxis.z;
            m.m30 = -Vector3.Dot(xaxis, pos);
            m.m31 = -Vector3.Dot(yaxis, pos);
            m.m32 = -Vector3.Dot(zaxis, pos);
            m.m33 = 1.0f;

            return m;
        }


        public static Matrix4x4 CreatePerspectiveFieldOfView(float fieldOfView, 
            float zNear, float zFar, float aspectRatio)
        {
            fieldOfView = (float)(fieldOfView * Constants.ToRadians);

            if (fieldOfView <= 0.0f || fieldOfView >= (float)Math.PI)
                throw new ArgumentOutOfRangeException(nameof(fieldOfView));

            if (zNear <= 0.0f)
                throw new ArgumentOutOfRangeException(nameof(zNear));

            if (zFar <= 0.0f)
                throw new ArgumentOutOfRangeException(nameof(zFar));

            if (zNear >= zFar)
                throw new ArgumentOutOfRangeException(nameof(zNear));

            float yScale = (float)(1.0f / Math.Tan(fieldOfView * 0.5f));
            float xScale = yScale / aspectRatio;

            Matrix4x4 result = new Matrix4x4();

            result.m00 = xScale;
            
            result.m11 = yScale;
            var negFarRange = float.IsPositiveInfinity(zFar) ? -1.0f : zFar / (zNear - zFar);
            result.m22 = negFarRange;
            result.m23 = -1.0f;

            result.m32 = zNear * negFarRange;

            return result;
        }


        public static Matrix4x4 CreateTransformMatrix(Vector3 scale, Quaternion rot, Vector3 pos)
        {
            if (SIMD_Math.IsEnabled)
                return SIMD_Math.CreateTransformMatrix(scale,rot,pos);

            return CreateScaleMatrix(scale) * CreateRotationMatrix(rot) * CreateTranslationMatrix(pos);
        }

        public static Matrix4x4 CreateTransformMatrix(Matrix4x4 scaleMatrix,Matrix4x4 rotMatrix,Matrix4x4 transMatrix)
        {
            if (SIMD_Math.IsEnabled)
                return SIMD_Math.CreateTransformMatrix(scaleMatrix, rotMatrix, transMatrix);

            return scaleMatrix * rotMatrix * transMatrix;
        }

        public Vector3 Transform(Vector3 v) => Transform(v, this);

        public static Vector3 Transform(Vector3 v, Matrix4x4 m)
        {
            /*
            if (Vec128.IsEnabled)
                return Vec128.VectorTransform(v, m);
                */

            /* Transpose the matrix beforehand?
             * m.m00  m.m01  m.m02
             *   *      *      *    FMA
             * v.x    v.x    v.x
             *   +      +      +
             * v.y    v.y    v.y
             *   *      *      *    FMA
             * m.m10  m.m11  m.m22
             *   +      +      +
             * v.z    v.z    v.z
             *   *      *      *
             * m.m20  m.m21  m.m22
             */
            return new Vector3(
                v.x * m.m00 + v.y * m.m10 + v.z * m.m20 + m.m30,
                v.x * m.m01 + v.y * m.m11 + v.z * m.m21 + m.m31,
                v.x * m.m02 + v.y * m.m12 + v.z * m.m22 + m.m32);
        }

        public static Matrix4x4 Multiply(Matrix4x4 m1, Matrix4x4 m2)
        {
            return m1 * m2;
        }

        public static Matrix4x4 operator * (Matrix4x4 m1, Matrix4x4 m2)
        {
            if (SIMD_Math.IsEnabled)
                return SIMD_Math.MatrixMul(m1, m2);
            
            Matrix4x4 m = new Matrix4x4();

            m.m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20 + m1.m03 * m2.m30;
            m.m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20 + m1.m13 * m2.m30;
            m.m20 = m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20 + m1.m23 * m2.m30;
            m.m30 = m1.m30 * m2.m00 + m1.m31 * m2.m10 + m1.m32 * m2.m20 + m1.m33 * m2.m30;

            m.m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21 + m1.m03 * m2.m31;
            m.m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21 + m1.m13 * m2.m31;
            m.m21 = m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21 + m1.m23 * m2.m31;
            m.m31 = m1.m30 * m2.m01 + m1.m31 * m2.m11 + m1.m32 * m2.m21 + m1.m33 * m2.m31;

            // First row FMA, FMA, FMA, Mul
            m.m02 = m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22 + m1.m03 * m2.m32;
            m.m03 = m1.m00 * m2.m03 + m1.m01 * m2.m13 + m1.m02 * m2.m23 + m1.m03 * m2.m33;

            // Second row
            m.m12 = m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22 + m1.m13 * m2.m32;
            m.m13 = m1.m10 * m2.m03 + m1.m11 * m2.m13 + m1.m12 * m2.m23 + m1.m13 * m2.m33;

            // Third row
            m.m22 = m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22 + m1.m23 * m2.m32;
            m.m23 = m1.m20 * m2.m03 + m1.m21 * m2.m13 + m1.m22 * m2.m23 + m1.m23 * m2.m33;

            // Fourth row
            m.m32 = m1.m30 * m2.m02 + m1.m31 * m2.m12 + m1.m32 * m2.m22 + m1.m33 * m2.m32;
            m.m33 = m1.m30 * m2.m03 + m1.m31 * m2.m13 + m1.m32 * m2.m23 + m1.m33 * m2.m33;

            return m;
        }

        public float[] ToArray()
        {
            return new float[] {m00,m01,m02,m03,
                                m10,m11,m12,m13,
                                m20,m21,m22,m23,
                                m30,m31,m32,m33};
        }

        public override string ToString()
        {
            return $"{m00},{m10},{m20},{m30} {Environment.NewLine}" +
                   $"{m01},{m11},{m21},{m31} {Environment.NewLine}" +
                   $"{m02},{m12},{m22},{m32} {Environment.NewLine}" +
                   $"{m03},{m12},{m23},{m33}";
        }
    }
}
