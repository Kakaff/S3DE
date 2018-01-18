using OpenGL;
using S3DE.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_OGL_Renderer
{
    public sealed class OpenGL_Shader
    {
        static Dictionary<Type, OpenGL_Shader> loadedShaders = new Dictionary<Type, OpenGL_Shader>();

        string source;
        ShaderStage shaderStage;
        uint pointer;

        internal ShaderStage Stage => shaderStage;
        internal string Source => source;
        internal uint Pointer => pointer;

        bool isFreed = false;

        private OpenGL_Shader()
        {

        }

        void SetSource(string source) => this.source = source;
        void SetStage(ShaderStage stage) => this.shaderStage = stage;

        public static OpenGL_Shader Create(ShaderSource shadersource)
        {
            OpenGL_Shader shad = new OpenGL_Shader();

            shad.SetSource(shadersource.Source);
            shad.SetStage(shadersource.Stage);
            uint pointer = 0;

            switch (shadersource.Stage)
            {
                case ShaderStage.Vertex:
                    {
                        pointer = Gl.CreateShader(ShaderType.VertexShader);
                        break;
                    }
                case ShaderStage.Fragment:
                    {
                        pointer = Gl.CreateShader(ShaderType.FragmentShader);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Unknown/Unsupported ShaderStage");
                    }
            }

            shad.pointer = pointer;
            Gl.ShaderSource(pointer, new string[] {shad.Source});

            return shad;
        }

        internal void Free()
        {
            if (!isFreed)
            {
                Gl.DeleteShader(pointer);
                source = null;

                isFreed = true;
            } else
            {
                throw new ObjectDisposedException("Shader is already freed!");
            }
        }
    }
}
