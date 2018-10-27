using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Shaders
{
    public sealed partial class ShaderProgram
    {
        [DllImport("S3DECore.dll")]
        private static extern void Extern_Attach_Shader(IntPtr program, IntPtr shader);

        [DllImport("S3DECore.dll")]
        private static extern bool Extern_Link_Program(IntPtr program);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_Detach_Shader(IntPtr program, IntPtr shader);

        [DllImport("S3DECore.dll")]
        private static extern IntPtr Extern_Create_ShaderProgram();

        [DllImport("S3DECore.dll")]
        private static extern int Extern_GetUniformLocation(IntPtr handle, string name);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_Use_Program(IntPtr program);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniform1i(uint loc, int i1);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniform1ui(uint loc, uint ui1);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniform1f(uint loc, float f1);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniform2i(uint loc, int i1, int i2);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniform2ui(uint loc, uint ui1, uint ui2);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniform2f(uint loc, float f1, float f2);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniform3i(uint loc, int i1,int i2, int i3);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniform3ui(uint loc, uint ui1, uint ui2, uint ui3);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniform3f(uint loc, float f1, float f2, float f3);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniform4i(uint loc, int i1, int i2, int i3, int i4);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniform4ui(uint loc, uint ui1, uint ui2, uint ui3, uint ui4);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniform4f(uint loc, float f1, float f2, float f3,float f4);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniformMatrixf4v(uint loc, uint count, bool transpose, float[] matr);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetUniformMatrixf3v(uint loc, uint count, bool transpose, float[] matr);
    }
}
