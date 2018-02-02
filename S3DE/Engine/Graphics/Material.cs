﻿using S3DE.Maths;
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
        bool usesTransMatrix, usesViewMatrix, usesProjectionMatrix;

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
            {
                _rMaterial.Compile_Internal();
                if (UsesTransformMatrix)
                    AddUniform("transform");
                if (UsesViewMatrix)
                    AddUniform("view");
                if (UsesProjectionMatrix)
                    AddUniform("projection");
                AddUserDefinedUniforms();
            }

            _rMaterial.UseRendererMaterial();
        }

        internal void SetSources()
        {
            _rMaterial.SetSource_Internal(GetSource(ShaderStage.Vertex));
            _rMaterial.SetSource_Internal(GetSource(ShaderStage.Fragment));
        }

        internal void SetTransformMatrix(Matrix4x4 m) => SetUniform("transform", m);
        internal void SetViewMatrix(Matrix4x4 m) => SetUniform("view", m);
        internal void SetProjectionMatrix(Matrix4x4 m) => SetUniform("projection", m);

        protected abstract void UpdateUniforms();

        internal void AddUniform(string uniformName)
        {
            try
            {
                _rMaterial.Internal_AddUniform(uniformName);
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

        protected void SetUniform(string uniformName, float value) => _rMaterial.Internal_SetUniformf(uniformName, value);
        protected void SetUniform(string uniformName, int value) => _rMaterial.Internal_SetUniformi(uniformName, value);
        protected void SetUniform(string uniformName, float[] value) => _rMaterial.Internal_SetUniformf(uniformName, value);
        protected void SetUniform(string uniformName, Vector3 value) => _rMaterial.Internal_SetUniformf(uniformName, value.ToArray());
        protected void SetUniform(string uniformName, Matrix4x4 value) => _rMaterial.Internal_SetUniform(uniformName, value);
    }
}