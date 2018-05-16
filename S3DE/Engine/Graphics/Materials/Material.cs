using S3DE.Engine.Entities;
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
        Renderer_Material _rActMaterial,_rDefMaterial,_rForMaterial,_rShadMaterial;
        protected abstract MaterialSource GetSource(ShaderStage stage,RenderPass pass);
        bool usesTransMatrix, usesViewMatrix, usesProjectionMatrix,usesRotationMatrix;
        bool supportsDeferred, supportsForward,supportShadow;

        bool isCreated;

        public bool SupportsShadowMapping
        {
            get => supportShadow;
            protected set => supportShadow = value;
        }

        public bool SupportsDeferredRendering
        {
            get => supportsDeferred;
            protected set => supportsDeferred = value;
        }

        public bool SupportsForwardRendering
        {
            get => supportsForward;
            protected set => supportsForward = value;
        }

        public bool UsesTransformMatrix
        {
            get => usesTransMatrix;
            protected set => usesTransMatrix = value;
        }

        public bool UsesViewMatrix
        {
            get => usesViewMatrix;
            protected set => usesViewMatrix = value;
        }

        public bool UsesProjectionMatrix
        {
            get => usesProjectionMatrix;
            protected set => usesProjectionMatrix = value;
        }

        public bool UsesRotationMatrix
        {
            get => usesRotationMatrix;
            protected set => usesRotationMatrix = value;
        }

        public bool SupportsRenderPass(RenderPass pass)
        {
            switch (pass)
            {
                case RenderPass.Deferred: return SupportsDeferredRendering;
                case RenderPass.Forward: return SupportsForwardRendering;
                case RenderPass.ShadowMap: return SupportsShadowMapping;
                default: return false;
            }
        }
        protected Material()
        {
            
        }

        protected void CreateRendererMaterial()
        {
            if (SupportsForwardRendering)
            {
                Console.WriteLine($"Created Forward RendererMaterial");
                _rForMaterial = Renderer.CreateMaterial_Internal(this.GetType(), RenderPass.Forward);
            }
            if (SupportsDeferredRendering)
            {
                Console.WriteLine($"Created Deferred RendererMaterial");
                _rDefMaterial = Renderer.CreateMaterial_Internal(this.GetType(), RenderPass.Deferred);
            }

            SetSources();
        }

        void CheckActiveRenderMatCompiled()
        {
            if (!_rActMaterial.IsCompiled)
            {
                _rActMaterial.Compile_Internal();
                if (UsesTransformMatrix)
                    AddUniform("transform");
                if (UsesViewMatrix)
                    AddUniform("view");
                if (UsesProjectionMatrix)
                    AddUniform("projection");
                if (UsesRotationMatrix)
                    AddUniform("rotation");
                AddUserDefinedUniforms();
            }
        }

        void CheckIsCreated()
        {
            if (!isCreated)
            {
                Console.WriteLine($"Creating RendererMaterials for {this.GetType().Name} in {this.GetType().Namespace}");
                CreateRendererMaterial();
                isCreated = true;
            }

        }

        public void UseMaterial(RenderPass pass)
        {
            CheckIsCreated();
            SetActiveRenderMaterial(pass);
            CheckActiveRenderMatCompiled();
            _rActMaterial.UseRendererMaterial();
            UpdateUniforms(pass);
        }

        internal void SetSources()
        {
            if (SupportsDeferredRendering)
            {
                _rDefMaterial.SetSource_Internal(GetSource(ShaderStage.Vertex, RenderPass.Deferred));
                _rDefMaterial.SetSource_Internal(GetSource(ShaderStage.Fragment, RenderPass.Deferred));
            }

            if (SupportsForwardRendering)
            {
                _rForMaterial.SetSource_Internal(GetSource(ShaderStage.Vertex, RenderPass.Forward));
                _rForMaterial.SetSource_Internal(GetSource(ShaderStage.Fragment, RenderPass.Forward));
            }
        }

        /// <summary>
        /// Switches the active RenderMaterial to the one supporting the current RenderPass.
        /// </summary>
        /// <param name="pass"></param>
        void SetActiveRenderMaterial(RenderPass pass)
        {
            switch (pass)
            {
                case RenderPass.Deferred: _rActMaterial = _rDefMaterial; break;
                case RenderPass.Forward: _rActMaterial = _rForMaterial; break;
                case RenderPass.ShadowMap: _rActMaterial = _rShadMaterial; break;
            }
        }

        internal void SetTransformMatrix(Matrix4x4 m) => SetUniform("transform", m);
        internal void SetViewMatrix(Matrix4x4 m) => SetUniform("view", m);
        internal void SetProjectionMatrix(Matrix4x4 m) => SetUniform("projection", m);
        internal void SetRotationMatrix(Matrix4x4 m) => SetUniform("rotation", m);

        internal void UpdateUniforms_Internal(RenderPass pass) => UpdateUniforms(pass);
        protected abstract void UpdateUniforms(RenderPass pass);

        internal void AddUniform(string uniformName)
        {
            try
            {
                _rActMaterial.Internal_AddUniform(uniformName);
            } catch (Exception ex)
            {
                throw new NullReferenceException($"Error getting uniform '{uniformName}' in '{GetType().Name}' | {ex.Message}");
            }
        }

        internal void AddUserDefinedUniforms()
        {
            string[] uniforms = GetUniforms();
            foreach (string s in uniforms)
                AddUniform(s);
        }

        protected virtual string[] GetUniforms() { return new string[0];}

        protected void SetUniform(string uniformName, float value) => _rActMaterial.Internal_SetUniformf(uniformName, value);
        protected void SetUniform(string uniformName, ILight light) => _rActMaterial.Internal_SetUniform(uniformName, light);
        protected void SetUniform(string uniformName, IDirectionalLight directionalLight) => _rActMaterial.Internal_SetUniform(uniformName, directionalLight);
        protected void SetUniform(string uniformName, int value) => _rActMaterial.Internal_SetUniformi(uniformName, value);
        protected void SetUniform(string uniformName, float[] value) => _rActMaterial.Internal_SetUniformf(uniformName, value);
        protected void SetUniform(string uniformName, Vector3 value) => _rActMaterial.Internal_SetUniform(uniformName, value);
        protected void SetUniform(string uniformName, Matrix4x4 value) => _rActMaterial.Internal_SetUniform(uniformName, value);
        protected void SetTexture(string samplerName, TextureUnit TextureUnit, ITexture texture) => _rActMaterial.Internal_SetTexture(samplerName,TextureUnit, texture);
        protected void SetTexture(string samplerName, ITexture texture)
        {
            bool isbound = texture.IsBound(out TextureUnit texUnit);
            if (!isbound)
                texUnit = texture.Bind();
            _rActMaterial.Internal_SetTexture(samplerName, texUnit, texture);
        }
    }
}
