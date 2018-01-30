using OpenGL;
using S3DE.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Maths;

namespace SampleGame.Sample_OGL_Renderer
{
    internal sealed class OpenGL_Material : Renderer_Material
    {
        OpenGL_ShaderProgram prog;

        ShaderSource VertexShader, FragmentShader;
        Dictionary<string, int> Uniforms;

        protected override void Compile()
        {
            prog = new OpenGL_ShaderProgram(OpenGL_Shader.Create(VertexShader), OpenGL_Shader.Create(FragmentShader));
            prog.Compile();
        }

        internal OpenGL_Material()
        {
            Uniforms = new Dictionary<string, int>();
        }

        protected override void SetSource(ShaderSource source)
        {
            switch (source.Stage)
            {
                case ShaderStage.Vertex:
                    {
                        VertexShader = source;
                        break;
                    }
                case ShaderStage.Fragment:
                    {
                        FragmentShader = source;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException(source.Stage + " is not yet implemented/supported");
                    }
            }
        }

        protected override void SetUniformf(string uniformName, float[] value) => prog.SetUniformf(uniformName, value);

        protected override void SetUniformf(string uniformName, float value) => prog.SetUniformf(uniformName, value);

        protected override void SetUniformi(string uniformName, int value) => prog.SetUniformi(uniformName, value);

        protected override void SetUniform(string uniformName, Matrix4x4 m) => prog.SetUniform(uniformName, m);

        protected override void UseMaterial() => prog.UseProgram();

        protected override void AddUniform(string uniformName) => prog.AddUniform(uniformName);
    }
}
