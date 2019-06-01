using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths.SIMD
{
    public static class SIMD_Math
    {
        public static bool IsEnabled => true;

        /// <summary>
        /// Multiplies 2 4x4 matricies.
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix4x4 MatrixMul(Matrix4x4 m1, Matrix4x4 m2)
        {
            Matrix4x4 res = new Matrix4x4();
            unsafe
            {
                if (Vec256.IsEnabled)
                    S3DECore.Math.Vecf256.MatrixMultiply(&m1.m00, &m2.m00, &res.m00);
            }
            
            return res;
        }

        public static Matrix4x4 QuaternionToRotationMatrix(Quaternion quat)
        {
            Matrix4x4 res = new Matrix4x4();

            unsafe
            {
                if (Vec128.IsEnabled)
                    S3DECore.Math.Vecf128.CreateRotationMatrix(&quat.x, &res.m00);
            }
            return res;
        }

        public static Matrix4x4 CreateTransformMatrix(Vector3 pos, Quaternion rot, Vector3 scale)
        {
            Matrix4x4 res = new Matrix4x4();

            unsafe
            {
                S3DECore.Math.Vecf128.CreateTransformMatrix(&scale.x, &rot.x, &pos.x, &res.m00);
            }

            return res;
        }

        public static Matrix4x4 CreateTransformMatrix(Matrix4x4 scale, Matrix4x4 rot, Matrix4x4 trnsl) 
        {
            Matrix4x4 res = new Matrix4x4();

            unsafe
            {
                S3DECore.Math.Vecf256.CreateTransformMatrix(&scale.m00, &rot.m00, &trnsl.m00, &res.m00);
            }

            return res;
        }
    }
}
