using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public abstract class Renderer_Material
    {
        protected abstract void SetSource(ShaderStage stage, ShaderSource source);
        protected abstract void Compile();

        protected abstract void UseMaterial();

        protected abstract void SetTransformMatrix(Matrix4x4 m);
        protected abstract void SetViewMatrix(Matrix4x4 m);
        protected abstract void SetProjectionMatrix(Matrix4x4 m);

        protected abstract void SetUniformf(string uniformName, float value);
        protected abstract void SetUniformi(string uniformName, int value);
        protected abstract void SetUniform(string uniformName, float[] value);

        internal void Internal_SetUniformf(string uniformName, float value) => SetUniformf(uniformName, value);
        internal void Internal_SetUniformi(string uniformName, int value) => SetUniformi(uniformName, value);
        internal void Internal_SetUniform(string uniformName, float[] value) => SetUniform(uniformName, value);

        internal void UseRendererMaterial() => UseMaterial();
        internal void SetSource_Internal(ShaderStage stage, ShaderSource source) => SetSource(stage, source);
    }
}
