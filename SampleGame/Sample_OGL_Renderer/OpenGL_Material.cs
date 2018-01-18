using OpenGL;
using S3DE.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_OGL_Renderer
{
    public sealed class OpenGL_Material : Renderer_Material
    {
        OpenGL_ShaderProgram prog;

        protected override void Compile()
        {
            //Create the individual shaders from the shadersources.
            //Link the shaders to the shaderprogram.

            //compile the shaderprogram.
            throw new NotImplementedException();
        }

        protected override void SetSource(ShaderStage stage, ShaderSource source)
        {
            throw new NotImplementedException();
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

        protected override void UseMaterial()
        {
            throw new NotImplementedException();
        }
    }
}
