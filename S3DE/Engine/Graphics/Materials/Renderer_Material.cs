using S3DE.Engine.Graphics.Lights;
using S3DE.Engine.Graphics.Textures;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.Materials
{
    public abstract class Renderer_Material
    {
        protected uint identifier;

        public bool IsCompiled {get; set;}
        static Renderer_Material CurrentlyBoundRendererMaterial;

        public uint Identifier
        {
            get
            {
                if (!IsCompiled)
                    Compile_Internal();
                return identifier;
            }
        }

        protected abstract void SetSource(MaterialSource source);
        protected abstract void Compile();

        protected abstract void UseMaterial();

        protected abstract void AddUniform(string uniformName);
        protected abstract void AddUniformBlock(string blockName);
        protected abstract int GetUniformBlockIndex(string name);

        protected abstract void SetUniformf(string uniformName, float value);
        protected abstract void SetUniformi(string uniformName, int value);
        protected abstract void SetUniformf(string uniformName, float[] value);
        protected abstract void SetUniform(string uniformName, Matrix4x4 m);
        protected abstract void SetUniform(string uniformName, Vector3 v);
        protected abstract void SetTexture(string uniformName, TextureUnit textureUnit, ITexture texture);
        protected abstract void SetUniformBlock(string blockName, int bindingPoint);
        //protected abstract void SetUniform(string uniformName, ILight light);
        //protected abstract void SetUniform(string uniformName, IDirectionalLight directionalLight);

        internal void Internal_AddUniform(string uniformName) => AddUniform(uniformName);
        internal void Internal_AddUniformBlock(string blockName) => AddUniformBlock(blockName);
        internal void Internal_SetUniformf(string uniformName, float value) => SetUniformf(uniformName, value);
        internal void Internal_SetUniformi(string uniformName, int value) => SetUniformi(uniformName, value);
        internal void Internal_SetUniformf(string uniformName, float[] value) => SetUniformf(uniformName, value);
        internal void Internal_SetUniform(string uniformName, Matrix4x4 m) => SetUniform(uniformName, m);
        internal void Internal_SetUniform(string uniformName, Vector3 v) => SetUniform(uniformName, v);
        internal void Internal_SetTexture(string uniformName, TextureUnit textureUnit, ITexture texture) => SetTexture(uniformName, textureUnit, texture);

        internal int GetUniformBlockIndex_Internal(string name) => GetUniformBlockIndex(name);

        internal void Internal_SetUniformBlock(string blockName, int bindingPoint) => SetUniformBlock(blockName, bindingPoint);

        internal void Internal_SetUniform(string uniformName, ILight light)
        {
            SetUniformf(uniformName + ".intensity", light.Intensity);
            SetUniform(uniformName + ".color", (Vector3)light.Color);
        }
        internal void Internal_SetUniform(string uniformName, IDirectionalLight directionalLight)
        {
            SetUniform(uniformName + ".direction", directionalLight.LightDirection);
            Internal_SetUniform(uniformName, (ILight)directionalLight);
        }

        internal void Compile_Internal() { Compile(); IsCompiled = true;}

        internal void UseRendererMaterial() {
            if (CurrentlyBoundRendererMaterial == null || CurrentlyBoundRendererMaterial.Identifier != Identifier)
            {
                UseMaterial();
                CurrentlyBoundRendererMaterial = this;
            }
        }

        internal void SetSource_Internal(MaterialSource source) => SetSource(source);
    }
}
