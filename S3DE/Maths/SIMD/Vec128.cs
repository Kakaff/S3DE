using System.Runtime.InteropServices;

namespace S3DE.Maths.SIMD
{
    internal static class VectorExtensions {

        public static bool IsEnabled(VectorExtension extension) => true;
    }

    internal enum VectorExtension
    {
        SSE1,
        SSE2,
        SSE3,
        SSE4,
        SSE4_1,
        SSE4_2,
        FMA3,
        AVX,
        AVX2
    }

    public static class Vec128
    {
        static bool shouldUse = true,isSupported = true;

        public static bool IsSupported => isSupported;
        public static bool IsEnabled
        {
            get => isSupported & shouldUse;
            set => shouldUse = value;
        }

        /// <summary>
        /// FMA3
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="f3"></param>
        /// <param name="r"></param>
        public static unsafe void MulAdd(float* f1, float* f2, float* f3, float*r)
        {
            Extern_Vecf128_FastMulAdd(f1, f2, f3, r);
        }
        /// <summary>
        /// FMA3
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="f3"></param>
        /// <param name="r"></param>
        public static unsafe void MulSub(float* f1, float* f2, float* f3, float* r)
        {
            Extern_Vecf128_FastMulSub(f1, f2, f3, r);
        }
        /// <summary>
        /// SSE
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="r"></param>
        public static unsafe void Mul(float* f1, float* f2, float* r)
        {
            Extern_Vecf128_FastMul(f1, f2, r);
        }

        /// <summary>
        /// SSE
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="r"></param>
        public static unsafe void Mul(float* f1, float f2, float* r)
        {
            Extern_Vecf128_FastMul_1(f1, &f2, r);
        }
        /// <summary>
        /// SSE
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="r"></param>
        public static unsafe void Add(float* f1, float* f2, float* r)
        {
            Extern_Vecf128_FastAdd(f1, f2, r);
        }
        /// <summary>
        /// SEE
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="r"></param>
        public static unsafe void Sub(float* f1, float* f2, float* r)
        {
            Extern_Vecf128_FastSub(f1, f2, r);
        }

        public static Matrix4x4 CreateRotationMatrix(Quaternion q)
        {
            Matrix4x4 m = new Matrix4x4();
            unsafe
            {
                Extern_Vecf128_QuatToRotMatrix(&q.x, &m.m00);
            }
            return m;
        }

        #region SSE_Functions
        [DllImport("S3DECore.dll")]
        private static unsafe extern void Extern_Vecf128_QuatToRotMatrix(float* quat, float* resMatr);
        #endregion

        #region FMA3
        [DllImport("S3DECore.dll")]
        private static unsafe extern void Extern_Vecf128_FastMulAdd(float* f1, float* f2, float* f3, float* r);
        [DllImport("S3DECore.dll")]
        private static unsafe extern void Extern_Vecf128_FastMulSub(float* f1, float* f2, float* f3, float* r);
        #endregion

        #region SSE
        [DllImport("S3DECore.dll")]
        private static unsafe extern void Extern_Vecf128_FastMul(float* f1, float* f2, float* rf);

        [DllImport("S3DECore.dll")]
        private static unsafe extern void Extern_Vecf128_FastMul_1(float* f1, float* f2, float* rf);

        [DllImport("S3DECore.dll")]
        private static unsafe extern void Extern_Vecf128_FastAdd(float* f1, float* f2, float* rf);

        [DllImport("S3DECore.dll")]
        private static unsafe extern void Extern_Vecf128_FastSub(float* f1, float* f2, float* rf);
        #endregion
    }
}
