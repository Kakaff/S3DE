﻿using OpenGL;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Graphics.OpGL.OpenGL_Renderer;

namespace S3DE.Engine.Graphics.OpGL
{
    internal sealed class OpenGL_ShaderProgram
    {
        OpenGL_Shader vertexShader;
        OpenGL_Shader fragmentShader;
        Dictionary<string, int> Uniforms;
        uint pointer;

        static OpenGL_ShaderProgram _CurrentlyBoundShaderProgram = null;

        internal uint Pointer => pointer;

        internal OpenGL_ShaderProgram(OpenGL_Shader VertexShader, OpenGL_Shader FragmentShader)
        {
            this.vertexShader = VertexShader;
            this.fragmentShader = FragmentShader;
            Uniforms = new Dictionary<string, int>();
            pointer = Gl.CreateProgram();
        }

        internal void Compile()
        {
            if (!vertexShader.IsCompiled)
                vertexShader.Compile();

            if (!fragmentShader.IsCompiled)
                fragmentShader.Compile();

            Gl.AttachShader(pointer, vertexShader.Pointer);
            Gl.AttachShader(pointer, fragmentShader.Pointer);

            Gl.LinkProgram(pointer);
        }

        internal void UseProgram()
        {
            if (_CurrentlyBoundShaderProgram == null || _CurrentlyBoundShaderProgram.pointer != this.pointer)
            {
                _CurrentlyBoundShaderProgram = this;
                Gl.UseProgram(pointer);
                TestForGLErrors();
            }
        }
        
        internal void Dispose()
        {
            //Free all shaders.
        }

        internal int GetUniformLocation(string uniformName)
        {
            if (Uniforms.TryGetValue(uniformName, out int _loc))
                return _loc;
            else
                throw new ArgumentNullException($"Unknown Uniform '{uniformName}'");
        }

        internal void SetUniformf(string uniformName, float value)
        {
            Gl.Uniform1(GetUniformLocation(uniformName), value);
            TestForGLErrors();
        }

        internal void SetUniformf(string uniformName, float[] values)
        {
            Gl.Uniform1(GetUniformLocation(uniformName), values);
            TestForGLErrors();
        }

        internal void SetUniformi(string uniformName, int value)
        {
            Gl.Uniform1(GetUniformLocation(uniformName), value);
            TestForGLErrors();
        }

        internal void SetUniformi(string uniformName, int[] value)
        {
            Gl.Uniform1(GetUniformLocation(uniformName), value);
            TestForGLErrors();
        }

        internal void SetUniform(string uniformName, Matrix4x4 m)
        {
            Gl.UniformMatrix4(GetUniformLocation(uniformName),false,m.ToArray());
            TestForGLErrors();
        }

        internal void SetUniform(string uniformName, Vector3 v)
        {
            Gl.Uniform3(GetUniformLocation(uniformName), v.ToArray());
            TestForGLErrors();
        }
        
        internal void AddUniform(string uniformName)
        {
            UseProgram();

            int loc = Gl.GetUniformLocation(Pointer, uniformName);
            if (loc != -0x1) {
                Uniforms.Add(uniformName, loc);
            }
            else
                throw new ArgumentException($"Unable to find uniform: {uniformName}");
            TestForGLErrors();
        }
        
        
    }
}