using System;
using System.Runtime.InteropServices;

namespace S3DE.Graphics.FrameBuffers
{
    partial class FrameBuffer
    {
        [DllImport("S3DECore.dll")]
        private static extern IntPtr Extern_FrameBuffer_Create();

        [DllImport("S3DECore.dll")]
        private static extern bool Extern_FrameBuffer_IsComplete(IntPtr handle);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_FrameBuffer_Bind(IntPtr handle);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_FrameBuffer_Unbind();

        [DllImport("S3DECore.dll")]
        private static extern void Extern_FrameBuffer_Clear(IntPtr handle, int ClearBit);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_FrameBuffer_AddTextureAttachment2D(IntPtr tex, uint attachmentLoc, int level);

        [DllImport("S3DECore.dll")]
        private static extern int Extern_FrameBuffer_CheckStatus();
    }
}