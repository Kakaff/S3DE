using S3DE.Graphics.FrameBuffers;
using System.Runtime.InteropServices;

namespace S3DE.Graphics
{
    static partial class Renderer
    {
        [DllImport("S3DECore.dll")]
        private static extern void InitGlew();

        [DllImport("S3DECore.dll")]
        internal static extern void Extern_Clear(ClearBufferBit bit);

        [DllImport("S3DECore.dll")]
        internal static extern void Extern_Enable(uint v);

        [DllImport("S3DECore.dll")]
        internal static extern void Extern_Disable(uint v);

        [DllImport("S3DECore.dll")]
        private static extern uint Extern_CheckGLErrors();

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetViewPortSize(uint width, uint height);
    }
}
