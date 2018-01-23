using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public enum ShaderStage
    {
        Vertex,
        Tessalation,
        Tessalation_Evaluation,
        Geometry,
        Fragment
    }

    public abstract class Material
    {
        Renderer_Material _rMaterial;
        protected abstract ShaderSource GetSource(ShaderStage stage);

        protected Material()
        {
            CreateRendererMaterial();
        }

        internal void CreateRendererMaterial()
        {
            _rMaterial = Renderer.CreateMaterial(this.GetType());
            SetSources();
        }

        public void UseMaterial()
        {
            if (!_rMaterial.IsCompiled)
                _rMaterial.Compile_Internal();

            UpdateUniforms();
            _rMaterial.UseRendererMaterial();
        }

        internal void SetSources()
        {
            _rMaterial.SetSource_Internal(GetSource(ShaderStage.Vertex));
            _rMaterial.SetSource_Internal(GetSource(ShaderStage.Fragment));
        }

        internal void SetTransformMatrix(Matrix4x4 m) => _rMaterial.SetTransformMatrix_Internal(m);
        internal void SetViewMatrix(Matrix4x4 m) => _rMaterial.SetViewMatrix_Internal(m);
        internal void SetProjectionMatrix(Matrix4x4 m) => _rMaterial.SetProjectionMatrix_Internal(m);

        protected abstract void UpdateUniforms();
        //Protected method for adding uniforms.

        protected void SetUniform(string uniformName, float value) => _rMaterial.Internal_SetUniformf(uniformName, value);
        protected void SetUniform(string uniformName, int value) => _rMaterial.Internal_SetUniformi(uniformName, value);
        protected void SetUniform(string uniformName, float[] value) => _rMaterial.Internal_SetUniform(uniformName, value);
    }
}
