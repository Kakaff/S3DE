using S3DE.Engine.Graphics.Lights;
using S3DE.Engine.Graphics.Materials;
using S3DE.Engine.Graphics.Textures;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.Shaders
{
    public struct ShaderProgram
    {
        uint identifier;

        public uint Identifier => identifier;
        public bool IsBound => Renderer.BoundShaderProgram == identifier;
        public bool IsCompiled => Renderer.Check_ShaderProgram_IsCompiled(this);

        internal ShaderProgram(uint id)
        {
            identifier = id;
        }

        public void SetSource(ShaderStage stage, string src) => Renderer.ShaderProgram_SetSource(this, stage, src);
        public void Bind() => Renderer.ShaderProgram_Bind(this);
        public void Compile() => Renderer.Compile_ShaderProgram(this);

        public int GetUniformIndex(string name) => Renderer.GetUniformLocation(name);
        public uint GetUniformBlockIndex(string name) => Renderer.GetUniformBlockLocation(name);

        public void SetUniform(int index, float value) => Renderer.Set_Uniform(index,value);
        public void SetUniform(int index, int value) => Renderer.Set_Uniform(index,value);
        public void SetUniform(int index, float[] values) => Renderer.Set_Uniform(index, values);
        public void SetUniform(int index, Matrix4x4 m) => Renderer.Set_Uniform(index,m);
        public void SetUniform(int index, System.Numerics.Vector3 v) => Renderer.Set_Uniform(index, v);
        public void SetUniform(int index, ILight light) => Renderer.Set_Uniform(index, light);
        public void SetUniform(int index, IDirectionalLight dirLight) => Renderer.Set_Uniform(index, dirLight);

        public void SetUniform(string name, float value) => Renderer.Set_Uniform(name, value);
        public void SetUniform(string name, int value) => Renderer.Set_Uniform(name, value);
        public void SetUniform(string name, float[] values) => Renderer.Set_Uniform(name, values);
        public void SetUniform(string name, Matrix4x4 m) => Renderer.Set_Uniform(name, m);
        public void SetUniform(string name, System.Numerics.Vector3 v) => Renderer.Set_Uniform(name, v);
        public void SetUniform(string name, ILight light) => Renderer.Set_Uniform(name, light);
        public void SetUniform(string name, IDirectionalLight dirLight) => Renderer.Set_Uniform(name, dirLight);

        public void SetTextureSampler(int index, ITexture texture)
        {
            if (!texture.IsBound(out TextureUnit tu))
                tu = texture.Bind();
            SetUniform(index, (int)tu);
        }

        public void SetTextureSampler(string name, ITexture texture)
        {
            if (!texture.IsBound(out TextureUnit tu))
                tu = texture.Bind();
            SetUniform(name, (int)tu);
        }

        public void SetTextureSamplers(string[] names, ITexture[] textures)
        {
            if (names.Length != textures.Length)
                throw new ArgumentException("The names array has to be the same length as the texture array!");

            TextureUnits.BindTextures(textures);
            for (int i = 0; i < names.Length; i++)
                SetUniform(names[i], (int)textures[i].BoundTextureUnit);
        }

        public void SetTextureSamplers(int[] locations, ITexture[] textures)
        {
            if (locations.Length != textures.Length)
                throw new ArgumentException("The names array has to be the same length as the texture array!");

            TextureUnits.BindTextures(textures);
            for (int i = 0; i < locations.Length; i++)
                SetUniform(locations[i], (int)textures[i].BoundTextureUnit);
        }

        public void SetUniformBlock(int index, UniformBuffer buffer) => Renderer.Set_UniformBlock(index, buffer);
        public void SetUniformBlock(string name, UniformBuffer buffer) => Renderer.Set_UniformBlock(name, buffer);
        public void SetUniformBlocks(string[] names, UniformBuffer[] buffers) => Renderer.Set_UniformBlocks(names, buffers);
        public void SetUniformBlocks(int[] locations, UniformBuffer[] buffers) => Renderer.Set_UniformBlocks(locations, buffers);
    }
}
