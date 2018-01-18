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

        internal void CreateRendererMaterial()
        {
            _rMaterial = Renderer.CreateMaterial(this.GetType());
            SetSources();
        }

        public void UseMaterial()
        {
            UpdateUniforms();
            _rMaterial.UseRendererMaterial();
        }


        internal void SetSources()
        {
            _rMaterial.SetSource_Internal(ShaderStage.Vertex, GetSource(ShaderStage.Vertex));
        }


        internal void SetTransformMatrix(Matrix4x4 m)
        {

        }

        internal void SetViewMatrix(Matrix4x4 m)
        {

        }

        internal void SetProjectionMatrix(Matrix4x4 m)
        {

        }

        protected abstract void UpdateUniforms();
        //Protected method for adding uniforms.

        protected void SetUniform(string uniformName, float value) => _rMaterial.Internal_SetUniformf(uniformName, value);
        protected void SetUniform(string uniformName, int value) => _rMaterial.Internal_SetUniformi(uniformName, value);
        protected void SetUniform(string uniformName, float[] value) => _rMaterial.Internal_SetUniform(uniformName, value);
    }
}
