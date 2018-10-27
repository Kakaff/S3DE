﻿using S3DE.Utility;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace S3DE.Graphics.Shaders
{
    public sealed partial class ShaderProgram
    {
        bool isBound = false;

        static ShaderProgram boundShadProg = null;

        IntPtr handle;

        List<Shader> attachedShaders = new List<Shader>();

        public ShaderProgram()
        {
            handle = Extern_Create_ShaderProgram();
        }
        
        public ShaderProgram(params Shader[] shaders)
        {
            handle = Extern_Create_ShaderProgram();
            for (int i = 0; i < shaders.Length; i++)
                AttachShader(shaders[i]);
            
            LinkShader();
            DetachShaders();
        }

        public void AttachShader(Shader s)
        {
            if (!s.IsCompiled && !s.Compile())
                Console.WriteLine("Failed to compile shader");

            if (s.IsCompiled)
            {
                Extern_Attach_Shader(handle, s.Handle);
                attachedShaders.Add(s);
            }
        }

        public void DetachShaders()
        {
            for (int i = 0; i < attachedShaders.Count; i++)
                Extern_Detach_Shader(handle, attachedShaders[i].Handle);
        }

        public bool LinkShader()
        {
            bool res = Extern_Link_Program(handle);
            if (!res) //Throw Shader linking exception in the future.
                Console.WriteLine("Failed linking shaderprogram");
            return res;
        }

        public void UseProgram()
        {
            if (!isBound)
            {
                if (boundShadProg != null)
                    boundShadProg.isBound = false;
                boundShadProg = this;
                Extern_Use_Program(handle);
                isBound = true;
            }
        }

        public void SetUniform_1(uint location, int i1)
        {
            Extern_SetUniform1i(location, i1);
        }

        public void SetUniform_2(uint location, int i1, int i2)
        {
            Extern_SetUniform2i(location, i1, i2);
        }

        public void SetUniform_3(uint location, int i1, int i2, int i3)
        {
            Extern_SetUniform3i(location, i1, i2, i3);
        }

        public void SetUniform_4(uint location, int i1, int i2, int i3, int i4)
        {
            Extern_SetUniform4i(location, i1, i2, i3, i4);
        }

        public void SetUniform_1(uint location, uint ui1)
        {
            Extern_SetUniform1ui(location, ui1);
        }

        public void SetUniform_2(uint location, uint ui1, uint ui2)
        {
            Extern_SetUniform2ui(location, ui1, ui2);
        }

        public void SetUniform_3(uint location, uint ui1, uint ui2, uint ui3)
        {
            Extern_SetUniform3ui(location, ui1, ui2, ui3);
        }

        public void SetUniform_4(uint location, uint ui1, uint ui2, uint ui3, uint ui4)
        {
            Extern_SetUniform4ui(location, ui1, ui2, ui3, ui4);
        }

        public void SetUniform_1(uint location, float value)
        {
            Extern_SetUniform1f(location, value);
        }

        public void SetUniform_2(uint location, float f1, float f2)
        {
            Extern_SetUniform2f(location, f1, f2);
        }

        public void SetUniform_3(uint location, float f1, float f2, float f3)
        {
            Extern_SetUniform3f(location, f1, f2, f3);
        }

        public void SetUniform_4(uint location, float f1, float f2, float f3, float f4)
        {
            Extern_SetUniform4f(location, f1, f2, f3, f4);
        }

        public void SetUniform(uint location, Vector2 v) => SetUniform_2(location, v.X, v.Y);

        public void SetUniform(uint location, Vector3 v) => SetUniform_3(location, v.X, v.Y, v.Z);

        public void SetUniform(uint location, Vector4 v) => SetUniform_4(location, v.X, v.Y, v.Z, v.W);

        public void SetUniform(uint location, Quaternion q) => SetUniform_4(location, q.X, q.Y, q.Z, q.W);

        public void SetUniform(uint location, S3DE.Maths.Matrix4x4 m)
        {
            float[] arr = m.ToArray();
            using (PinnedMemory pm = new PinnedMemory(arr))
                Extern_SetUniformMatrixf4v(location, 1, false, arr);
        }

        public void SetUniform_1v(uint location, int[] values)
        {
            throw new NotImplementedException();
        }

        public void SetUniform_1v(uint location, uint[] values)
        {
            throw new NotImplementedException();
        }

        public void SetUniform_1v(uint location, float[] values)
        {
            throw new NotImplementedException();
        }

        public void SetUniform_2v(uint location, int[] values)
        {
            throw new NotImplementedException();
            if (!IsMultipleOf(values.Length, 2))
                throw new ArgumentException("The length of the provided array must be a multiple of 2!");
        }

        public void SetUniform_2v(uint location, uint[] values)
        {
            throw new NotImplementedException();
            if (!IsMultipleOf(values.Length, 2))
                throw new ArgumentException("The length of the provided array must be a multiple of 2!");
        }

        public void SetUniform_2v(uint location, float[] values)
        {
            throw new NotImplementedException();
            if (!IsMultipleOf(values.Length, 2))
                throw new ArgumentException("The length of the provided array must be a multiple of 2!");
        }

        bool IsMultipleOf(int x, int n) => (float)x % (float)n == 0;

        public int GetUniformLocation(string uniformName)
        {
            int loc = 0;
            using (PinnedMemory pm = new PinnedMemory(uniformName))
                loc = Extern_GetUniformLocation(handle, uniformName);

            return loc;
        }
    }
}
