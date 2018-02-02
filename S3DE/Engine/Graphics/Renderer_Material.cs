﻿using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public abstract class Renderer_Material
    {
        public bool IsCompiled { get; set; }
        protected abstract void SetSource(ShaderSource source);
        protected abstract void Compile();

        protected abstract void UseMaterial();

        protected abstract void AddUniform(string uniformName);

        protected abstract void SetUniformf(string uniformName, float value);
        protected abstract void SetUniformi(string uniformName, int value);
        protected abstract void SetUniformf(string uniformName, float[] value);
        protected abstract void SetUniform(string uniformName, Matrix4x4 m);

        internal void Internal_AddUniform(string uniformName) => AddUniform(uniformName);
        internal void Internal_SetUniformf(string uniformName, float value) => SetUniformf(uniformName, value);
        internal void Internal_SetUniformi(string uniformName, int value) => SetUniformi(uniformName, value);
        internal void Internal_SetUniformf(string uniformName, float[] value) => SetUniformf(uniformName, value);
        internal void Internal_SetUniform(string uniformName, Matrix4x4 m) => SetUniform(uniformName, m);

        internal void Compile_Internal() { Compile(); IsCompiled = true;}
        internal void UseRendererMaterial() => UseMaterial();
        internal void SetSource_Internal(ShaderSource source) => SetSource(source);
    }
}