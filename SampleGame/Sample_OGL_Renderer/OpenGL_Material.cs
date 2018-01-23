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

        protected override void Compile()
        {
            prog = new OpenGL_ShaderProgram(OpenGL_Shader.Create(VertexShader), OpenGL_Shader.Create(FragmentShader));
            prog.Compile();
        }

        internal OpenGL_Material()
        {
            
        }

        protected override void SetProjectionMatrix(Matrix4x4 m)
        {
            //throw new NotImplementedException();
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

        protected override void SetTransformMatrix(Matrix4x4 m)
        {
            //SetUniform("transform",m);
            //throw new NotImplementedException();
        }

        protected override void SetUniform(string uniformName, float[] value)
        {
            throw new NotImplementedException();
        }

        protected override void SetUniformf(string uniformName, float value)
        {
            throw new NotImplementedException();
        }

        protected override void SetUniformi(string uniformName, int value)
        {
            throw new NotImplementedException();
        }

        protected override void SetViewMatrix(Matrix4x4 m)
        {
            //throw new NotImplementedException();
        }

        protected override void UseMaterial()
        {
            prog.UseProgram();
        }
    }
}
