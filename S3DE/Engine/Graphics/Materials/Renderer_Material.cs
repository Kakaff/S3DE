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
        public bool IsCompiled { get; set; }
        protected abstract void SetSource(MaterialSource source);
        protected abstract void Compile();

        protected abstract void UseMaterial();

        protected abstract void AddUniform(string uniformName);

        protected abstract void SetUniformf(string uniformName, float value);
        protected abstract void SetUniformi(string uniformName, int value);
        protected abstract void SetUniformf(string uniformName, float[] value);
        protected abstract void SetUniform(string uniformName, Matrix4x4 m);
        protected abstract void SetUniform(string uniformName, Vector3 v);
        protected abstract void SetTexture(string uniformName, int textureUnit, ITexture texture);
        //protected abstract void SetUniform(string uniformName, ILight light);
        //protected abstract void SetUniform(string uniformName, IDirectionalLight directionalLight);

        internal void Internal_AddUniform(string uniformName) => AddUniform(uniformName);
        internal void Internal_SetUniformf(string uniformName, float value) => SetUniformf(uniformName, value);
        internal void Internal_SetUniformi(string uniformName, int value) => SetUniformi(uniformName, value);
        internal void Internal_SetUniformf(string uniformName, float[] value) => SetUniformf(uniformName, value);
        internal void Internal_SetUniform(string uniformName, Matrix4x4 m) => SetUniform(uniformName, m);
        internal void Internal_SetUniform(string uniformName, Vector3 v) => SetUniform(uniformName, v);
        internal void Internal_SetTexture(string uniformName, int textureUnit, ITexture texture) => SetTexture(uniformName, textureUnit, texture);

        internal void Internal_SetUniform(string uniformName, ILight light)
        {
            SetUniformf(uniformName + ".intensity", light.Intensity);
            SetUniform(uniformName + ".color", (Vector3)light.Color);
        }
        internal void Internal_SetUniform(string uniformName, IDirectionalLight directionalLight)
        {
            SetUniform(uniformName + ".direction", directionalLight.LightDirection);
            Internal_SetUniform(uniformName, directionalLight);
        }

        internal void Compile_Internal() { Compile(); IsCompiled = true;}
        internal void UseRendererMaterial() => UseMaterial();
        internal void SetSource_Internal(MaterialSource source) => SetSource(source);
    }
}
