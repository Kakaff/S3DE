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
                    Extern_Vecf256_MatrixMul(&m1.m00,&m2.m00,&res.m00);

                if (Vec128.IsEnabled)
                    return Vec128.MatrixMul(m1, m2);
            }
            

            return res;
        }

        public static Matrix4x4 SSE_MatrixMul(Matrix4x4 m1, Matrix4x4 m2)
        {
            if (Vec128.IsEnabled)
                return Vec128.MatrixMul(m1, m2);

            return new Matrix4x4();
        }

        [DllImport("S3DECore.dll")]
        private static extern unsafe void Extern_Vecf256_MatrixMul(float* m1, float* m2, float* res);
    }
}
