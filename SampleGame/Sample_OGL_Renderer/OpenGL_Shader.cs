using OpenGL;
using S3DE.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_OGL_Renderer
{
    internal sealed class OpenGL_Shader
    {
        static Dictionary<Type, OpenGL_Shader> loadedShaders = new Dictionary<Type, OpenGL_Shader>();

        string source;
        ShaderStage shaderStage;
        uint pointer;
        bool isCompiled;

        internal ShaderStage Stage => shaderStage;
        internal string Source => source;
        internal uint Pointer => pointer;
        internal bool IsCompiled => isCompiled;

        bool isFreed = false;

        private OpenGL_Shader()
        {
            isCompiled = false;
        }

        void SetSource(string source) => this.source = source;
        void SetStage(ShaderStage stage) => this.shaderStage = stage;

        internal bool Compile()
        {
            Gl.CompileShader(pointer);

            int compileStatus;
            Gl.GetShader(pointer, ShaderParameterName.CompileStatus, out compileStatus);
            if (compileStatus != Gl.TRUE)
            {
                Gl.GetShader(pointer, ShaderParameterName.InfoLogLength, out int logLength);
                StringBuilder sb = new StringBuilder(logLength);
                Gl.GetShaderInfoLog(pointer, logLength, out int l, sb);
                throw new Exception("Failed to compile Shader | " + sb.ToString());
            }
            else
                return true;
        }

        public static OpenGL_Shader Create(ShaderType shadType,string source)
        {
            OpenGL_Shader shad = new OpenGL_Shader();

            shad.SetSource(source);
            shad.pointer = Gl.CreateShader(shadType);
            Gl.ShaderSource(shad.pointer, new string[] {source});
            return shad;
        }
        
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
            Gl.ShaderSource(shad.pointer,new string[] {shad.source});
            return shad;
        }

        internal void Free()
        {
            if (!isFreed)
            {
                Gl.DeleteShader(pointer);
                source = null;
                isFreed = true;
            }
        }
    }
}
