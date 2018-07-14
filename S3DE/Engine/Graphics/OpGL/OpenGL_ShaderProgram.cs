using OpenGL;
using S3DE.Engine.Graphics.Materials;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Graphics.OpGL.OpenGL_Renderer;

namespace S3DE.Engine.Graphics.OpGL
{
    internal sealed class OpenGL_ShaderProgram
    {
        class ShaderSource
        {
            string code;
            ShaderStage stage;

            bool isCompiled;
            uint identifier;
            
            public uint Identifier => identifier;
            public bool IsCompiled => isCompiled;
            public ShaderStage Stage => stage;
            public string Source => code;

            private ShaderSource() { }

            public ShaderSource(ShaderStage s, string src)
            {
                code = src;
                stage = s;
                isCompiled = false;
            }

            public void Compile()
            {
                identifier = Gl.CreateShader(OpenGL_Utility.Convert(Stage));
                OpenGL_Renderer.TestForGLErrors();

                Gl.ShaderSource(identifier, new string[] {Source});
                OpenGL_Renderer.TestForGLErrors();

                Gl.CompileShader(identifier);
                OpenGL_Renderer.TestForGLErrors();
                int compileStatus;
                Gl.GetShader(identifier, ShaderParameterName.CompileStatus, out compileStatus);
                OpenGL_Renderer.TestForGLErrors();
                if (compileStatus != Gl.TRUE)
                {
                    Gl.GetShader(identifier, ShaderParameterName.InfoLogLength, out int logLength);
                    OpenGL_Renderer.TestForGLErrors();
                    StringBuilder sb = new StringBuilder(logLength);
                    Gl.GetShaderInfoLog(identifier, logLength, out int l, sb);
                    OpenGL_Renderer.TestForGLErrors();
                    throw new Exception("Failed to compile Shader | " + sb.ToString());
                }
                else
                    isCompiled = true;
            }
        }
        
        static OpenGL_ShaderProgram boundShaderProgram;

        Dictionary<string, int> Uniforms;
        Dictionary<string, uint> UniformBlocks;
        ShaderSource[] sources;
        bool isCompiled;
        uint pointer;

        internal bool IsCompiled => isCompiled;
        internal uint Pointer => pointer;
        internal static OpenGL_ShaderProgram BoundShaderProgram => boundShaderProgram;

        internal OpenGL_ShaderProgram(uint identifier)
        {
            Uniforms = new Dictionary<string, int>();
            UniformBlocks = new Dictionary<string, uint>();
            pointer = identifier;
            sources = new ShaderSource[5];
        }

        internal void SetSource(ShaderStage stage,string src)
        {
            ShaderSource ss = new ShaderSource(stage, src);

            switch (stage)
            {
                case ShaderStage.Vertex:
                    {
                        sources[0] = ss;
                        break;
                    }
                case ShaderStage.Fragment:
                    {
                        sources[1] = ss;
                        break;
                    }
            }
        }

        internal void Compile()
        {
            Console.WriteLine("Compiling shaderprogram");

            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i] != null)
                {
                    sources[i].Compile();
                    Gl.AttachShader(pointer, sources[i].Identifier);
                    TestForGLErrors();
                }
            }
            Gl.LinkProgram(pointer);
            TestForGLErrors();
            isCompiled = true;
        }

        internal void UseProgram()
        {
            if (boundShaderProgram == null || boundShaderProgram.Pointer != Pointer)
            {
                Gl.UseProgram(Pointer);
                TestForGLErrors();
                boundShaderProgram = this;
            }
        }

        internal int GetUniformLocation(string uniformName)
        {
            if (Uniforms.TryGetValue(uniformName, out int _loc))
                return _loc;
            else
                return AddUniform(uniformName);
        }

        internal uint GetUniformBlockLocation(string uniformBlockName)
        {
            if (UniformBlocks.TryGetValue(uniformBlockName, out uint loc))
                return loc;
            else
                return AddUniformBlock(uniformBlockName);
        }

        internal void SetUniformf(string uniformName, float value)
        {
            Renderer.Set_Uniform(GetUniformLocation(uniformName), value);
        }

        internal void SetUniformf(string uniformName, float[] values)
        {
            Renderer.Set_Uniform(GetUniformLocation(uniformName), values);
        }

        internal void SetUniformi(string uniformName, int value)
        {
            Renderer.Set_Uniform(GetUniformLocation(uniformName), value);
        }

        internal void SetUniformi(string uniformName, int[] values)
        {
            Renderer.Set_Uniform(GetUniformLocation(uniformName), values);
        }

        internal void SetUniform(string uniformName, Matrix4x4 m)
        {
            Renderer.Set_Uniform(GetUniformLocation(uniformName),m);
        }

        internal void SetUniform(string uniformName, System.Numerics.Vector3 v)
        {
            Renderer.Set_Uniform(GetUniformLocation(uniformName), v);
        }

        internal void SetUniform(string uniformName,Color color)
        {
            Renderer.Set_Uniform(GetUniformLocation(uniformName), color);
        }

        internal void SetUniformBlock(string uniformBlockName,UniformBuffer buff)
        {
            Gl.UniformBlockBinding(Pointer, GetUniformBlockLocation(uniformBlockName), (uint)buff.BoundUniformBlockBindingPoint);
            TestForGLErrors();
        }
        
        internal int AddUniform(string uniformName)
        {
            UseProgram();

            int loc = Gl.GetUniformLocation(Pointer, uniformName);
            if (loc != -0x1)
                Uniforms.Add(uniformName, loc);
            else
                throw new ArgumentNullException($"Uniform: '{uniformName}' does not exist.");

            Console.WriteLine($"Found Uniform {uniformName} at location {loc}");
            return loc;
        }

        internal uint AddUniformBlock(string uniformBlockName)
        {
            uint loc = Gl.GetUniformBlockIndex(Pointer, uniformBlockName);
            TestForGLErrors();
            if (loc == Gl.INVALID_INDEX)
                throw new ArgumentNullException($"UniformBlock: '{uniformBlockName}' does not exist.");
            UniformBlocks.Add(uniformBlockName, loc);

            Console.WriteLine($"Found UniformBlock {uniformBlockName} at location {loc}");
            return loc;
        }
        
        
    }
}
