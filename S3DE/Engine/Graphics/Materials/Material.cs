using S3DE.Engine.Data;
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
        Renderer_Material _rActMaterial, _rDefMaterial, _rForMaterial, _rShadMaterial;
        protected abstract MaterialSource GetSource(ShaderStage stage, RenderPass pass);
        bool usesTransformMatrices, usesCameraMatrices;
        bool usesTransMatrix, usesViewMatrix, usesProjectionMatrix, usesRotationMatrix;
        bool supportsDeferred, supportsForward, supportShadow;

        bool isCreated;

        string cameraUniformBlockName = "Camera";
        string transformUniformBlockName = "Transform";

        S3DE_UniformBuffer transformBuffer, cameraBuffer;


        public S3DE_UniformBuffer Transform_UBO { set => transformBuffer = value; }
        public S3DE_UniformBuffer Camera_UBO { set => cameraBuffer = value; }

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

        public bool UsesTransformMatrices
        {
            get => usesTransformMatrices;
            protected set => usesTransformMatrices = value;
        }

        public bool UsesCameraMatrices
        {
            get => usesCameraMatrices;
            protected set => usesCameraMatrices = value;
        }

        public string CameraUniformBlockName
        {
            get => cameraUniformBlockName;
            protected set => SetCameraUniformBlockName(value);
        }

        public string TransformUniformBlockName
        {
            get => transformUniformBlockName;
            protected set => SetTransformUniformBlockName(value);
        }

        private void SetTransformUniformBlockName(string n)
        {
            //Check to see if material is already compiled.
            transformUniformBlockName = n;
        }

        private void SetCameraUniformBlockName(string n)
        {
            //Check to see if material is already compiled.
            cameraUniformBlockName = n;
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

                if (UsesCameraMatrices)
                    AddUniformBlock(CameraUniformBlockName);
                if (UsesTransformMatrices)
                    AddUniformBlock(TransformUniformBlockName);

                AddUserDefinedUniforms();
                AddUserDefinedUniformBlocks();
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
        internal void SetTransformBlock(S3DE_UniformBuffer ubo) => SetUniformBlock(TransformUniformBlockName, ubo);
        internal void SetCameraBlock(S3DE_UniformBuffer ubo) => SetUniformBlock(CameraUniformBlockName, ubo);

        internal void UpdateUniforms_Internal(RenderPass pass)
        {
            UpdateUniforms(pass);
        }
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

        internal void AddUniformBlock(string blockName)
        {
            try
            {
                _rActMaterial.Internal_AddUniformBlock(blockName);
            } catch (Exception ex)
            {
                throw new NullReferenceException($"Error getting UniformBlock '{blockName}' in {GetType().Name}' | {ex.Message}");
            }
        }

        internal void AddUserDefinedUniformBlocks()
        {
            string[] uniformBlocks = GetUniformBlocks();
            foreach (string s in uniformBlocks)
                AddUniformBlock(s);
        }

        internal void AddUserDefinedUniforms()
        {
            string[] uniforms = GetUniforms();
            foreach (string s in uniforms)
                AddUniform(s);
        }

        protected virtual string[] GetUniformBlocks() { return new string[0];}
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

        protected void SetUniformBlock(string blockName,S3DE_UniformBuffer buffer)
        {
            if (!buffer.IsBound)
                buffer.Bind();

            _rActMaterial.Internal_SetUniformBlock(blockName, buffer.BoundUniformBlockBinding);
            
        }
    }
}
