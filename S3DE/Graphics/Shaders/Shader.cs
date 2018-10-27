using S3DE.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Shaders
{
    public sealed class Shader : IDisposable
    {
        IntPtr handle;
        ShaderStage stage;
        bool isCompiled;
        string src;

        internal bool IsCompiled => isCompiled;
        internal IntPtr Handle => handle;
        internal ShaderStage Stage => stage;

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetShaderSource(IntPtr shad, string src);

        [DllImport("S3DECore.dll")]
        private static extern IntPtr Extern_CreateShader(ShaderStage stage);

        [DllImport("S3DECore.dll")]
        private static extern bool Extern_CompileShader(IntPtr shad);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_DeleteShader(IntPtr shader);

        private Shader() { }

        public Shader(ShaderStage stage)
        {
            handle = Extern_CreateShader(stage);
            this.stage = stage;
        }

        public Shader(ShaderStage stage, string src)
        {
            handle = Extern_CreateShader(stage);
            this.stage = stage;
            SetShaderSource(src);
            Compile();
        }

        public void SetShaderSource(string src)
        {
            using (PinnedMemory pm = new PinnedMemory(src))
            {
                this.src = String.Copy(src);
                Extern_SetShaderSource(handle, src);
            }
        }

        public bool Compile()
        {
            bool res = false;

            if (!isCompiled)
            {
                res = Extern_CompileShader(handle);

                if (!res)
                {
                    throw new Exception("Failed to compile shader! \n" +
                                        $"ShaderStage: {stage} \n" +
                                        "SourceCode: \n" +
                                        $"{src}");
                }
                else
                    isCompiled = true;
            } //Else throw exception, shader is already compiled.
            return res;
        }

        public void Dispose()
        {
            src = null;
            Extern_DeleteShader(handle);
        }
    }
}
