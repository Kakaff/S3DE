using OpenGL;
using S3DE.Engine.Graphics.Lights;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL
{
    public sealed partial class OpenGL_Renderer : Renderer
    {
        protected override void SetUniform(int loc, int value)
        {
            Gl.Uniform1(loc, value);
            TestForGLErrors();
        }

        protected override void SetUniform(int loc, int[] values)
        {
            Gl.Uniform1(loc, values);
            TestForGLErrors();
        }

        protected override void SetUniform(int loc, float value)
        {
            Gl.Uniform1(loc, value);
            TestForGLErrors();
        }

        protected override void SetUniform(int loc, float[] values)
        {
            Gl.Uniform1(loc, values);
            TestForGLErrors();
        }

        protected override void SetUniform(int loc, Maths.Matrix4x4 matrix)
        {
            Gl.UniformMatrix4(loc, false, matrix.ToArray());
            TestForGLErrors();
        }

        protected override void SetUniform(int loc, Vector3 vector)
        {
            Gl.Uniform3(loc, vector.ToArray());
            TestForGLErrors();
        }

        protected override void SetUniform(int loc, Color color)
        {
            SetUniform(loc, color.ToVector3());
        }


        protected override void SetUniform(int loc, ILight light)
        {
            SetUniform(loc, light.Color);
            SetUniform(loc + 1, light.Intensity);
        }

        protected override void SetUniform(int loc, IDirectionalLight dirLight)
        {
            SetUniform(loc, dirLight as ILight);
            SetUniform(loc + 2, dirLight.LightDirection);
        }

        protected override void SetUniformBlock(int loc, UniformBuffer buff)
        {
            buff.Bind();
            SetUniform(loc, buff.BoundUniformBlockBindingPoint);
        }

        protected override void SetUniformBlocks(int[] locations, UniformBuffer[] buffers)
        {
            if (locations.Length != buffers.Length)
                throw new ArgumentException("The Location and buffer arrays have to be the same length!");

            UniformBuffer.Bind(buffers);
            for (int i = 0; i < locations.Length; i++)
                SetUniform(locations[i], buffers[i].BoundUniformBlockBindingPoint);

        }

        protected override void SetUniform(string name, float value)
        {
            OpenGL_ShaderProgram.BoundShaderProgram.SetUniformf(name, value);
        }

        protected override void SetUniform(string name, int value)
        {
            OpenGL_ShaderProgram.BoundShaderProgram.SetUniformi(name, value);
        }

        protected override void SetUniform(string name, int[] values)
        {
            OpenGL_ShaderProgram.BoundShaderProgram.SetUniformi(name, values);
        }

        protected override void SetUniform(string name, float[] values)
        {
            OpenGL_ShaderProgram.BoundShaderProgram.SetUniformf(name, values);
        }

        protected override void SetUniform(string name, Maths.Matrix4x4 matrix)
        {
            OpenGL_ShaderProgram.BoundShaderProgram.SetUniform(name, matrix);
        }

        protected override void SetUniform(string name, Vector3 vector)
        {
            OpenGL_ShaderProgram.BoundShaderProgram.SetUniform(name, vector);
        }

        protected override void SetUniform(string name, Color color)
        {
            OpenGL_ShaderProgram.BoundShaderProgram.SetUniform(name, color);
        }
        
        protected override void SetUniform(string name, ILight light)
        {
            SetUniform(name+".color", light.Color);
            SetUniform(name + ".intensity", light.Intensity);
        }

        protected override void SetUniform(string name, IDirectionalLight dirLight)
        {
            SetUniform(name, dirLight as ILight);
            SetUniform(name + ".direction", dirLight.LightDirection);
        }

        protected override void SetUniformBlock(string name, UniformBuffer buff)
        {
            buff.Bind();
            OpenGL_ShaderProgram.BoundShaderProgram.SetUniformBlock(name, buff);
        }

        protected override void SetUniformBlocks(string[] names, UniformBuffer[] buffers)
        {
            UniformBuffer.Bind(buffers);
            for (int i = 0; i < names.Length; i++)
                OpenGL_ShaderProgram.BoundShaderProgram.SetUniformBlock(names[i], buffers[i]);
        }

        protected override uint GetUniform_BlockLocation(string uniformBlockName)
        {
            return OpenGL_ShaderProgram.BoundShaderProgram.GetUniformBlockLocation(uniformBlockName);
        }

        protected override int GetUniform_Location(string uniformName)
        {
            return OpenGL_ShaderProgram.BoundShaderProgram.GetUniformLocation(uniformName);
        }
    }
}
