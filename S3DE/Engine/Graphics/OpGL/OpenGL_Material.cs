﻿using OpenGL;
using S3DE.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Maths;
using S3DE.Engine.Graphics.Materials;
using S3DE.Engine.Graphics.Textures;

namespace S3DE.Engine.Graphics.OpGL
{
    internal sealed class OpenGL_Material : Renderer_Material
    {
        OpenGL_ShaderProgram prog;

        MaterialSource VertexShader, FragmentShader;
        Dictionary<string, int> Uniforms;
        Dictionary<string, int> UniformBlocks;

        protected override void Compile()
        {
            prog = new OpenGL_ShaderProgram(OpenGL_Shader.Create(VertexShader), OpenGL_Shader.Create(FragmentShader));
            prog.Compile();
            identifier = prog.Pointer;
        }

        internal OpenGL_Material()
        {
            Uniforms = new Dictionary<string, int>();
            UniformBlocks = new Dictionary<string, int>();
        }

        protected override void UseMaterial()
        {
            prog.UseProgram();
        }

        protected override int GetUniformBlockIndex(string name)
        {
            int index = (int)Gl.GetUniformBlockIndex(prog.Pointer, name);
            if (index < 0)
                throw new Exception();

            OpenGL_Renderer.TestForGLErrors();
            return index;
        }

        protected override void SetSource(MaterialSource source)
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

        protected override void SetUniform(string uniformName, System.Numerics.Vector3 v) => prog.SetUniform(uniformName, v);

        protected override void SetUniformBlock(string blockName, int bindingPoint)
        {
            if (UniformBlocks.TryGetValue(blockName,out int index))
            {
                Gl.UniformBlockBinding(prog.Pointer, (uint)index, (uint)bindingPoint);
                OpenGL_Renderer.TestForGLErrors();
            } else
            {
                throw new NullReferenceException($"UniformBlock {blockName} does not exist");
            }
        }

        protected override void AddUniform(string uniformName) => prog.AddUniform(uniformName);

        protected override void AddUniformBlock(string blockName)
        {
            int index = GetUniformBlockIndex(blockName);
            UniformBlocks.Add(blockName, index);
        }


        protected override void SetTexture(string uniformName, TextureUnit textureUnit, ITexture texture)
        {
            texture.Bind(textureUnit);
            SetUniformi(uniformName, (int)textureUnit);
        }
    }
}
