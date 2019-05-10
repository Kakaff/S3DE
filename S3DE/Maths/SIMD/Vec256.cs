using System.Runtime.InteropServices;

namespace S3DE.Maths.SIMD
{
    internal static class Vec256
    {
        static bool shouldUse = true, isSupported = true;

        public static bool IsSupported => isSupported;
        public static bool IsEnabled
        {
            get => isSupported & shouldUse;
            set => shouldUse = value;
        }

        [DllImport("S3DECore.dll")]
        private static unsafe extern void Extern_Vecf256_MatrixMul(float* m1, float* m2, float* res);

        public static unsafe Matrix4x4 MatrixMul(Matrix4x4 m1, Matrix4x4 m2)
        {
            Matrix4x4 res = new Matrix4x4();
            unsafe
            {
                Extern_Vecf256_MatrixMul(&m1.m00, &m2.m00, &res.m00);
            }

            return res;
        }
    }
}
