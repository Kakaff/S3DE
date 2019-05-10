using S3DE.Components;
using S3DE.Graphics.Shaders;
using S3DE.Graphics.Textures;
using S3DE.Maths;
using System;
using System.Collections.Generic;

namespace S3DE.Graphics
{
    public abstract class Material
    {
        private sealed class Material_ShaderProgram
        {
            Dictionary<string, uint> uniformLocations;
            ShaderProgram sp;

            internal ShaderProgram ShaderProg => sp;
            internal Material_ShaderProgram(ShaderProgram sp)
            {
                this.sp = sp;
                uniformLocations = new Dictionary<string, uint>();
            }

            internal void Use()
            {
                sp.UseProgram();
            }

            internal uint GetUniformLocation(string name)
            {
                uint loc = 0;
                if (!uniformLocations.TryGetValue(name,out loc))
                {
                    int l = sp.GetUniformLocation(name);
                    if (l < 0)
                        throw new Exception($"Uniform {name} does not exist!");
                    loc = (uint)l;
                    uniformLocations.Add(name, loc);
                }

                return loc;
            }
        }

        protected sealed class MaterialSource
        {
            ShaderStage stage;
            string src;

            public string Source => src;
            public ShaderStage Stage => stage;

            private MaterialSource() { }

            public MaterialSource(ShaderStage stage, string src)
            {
                this.src = src;
                this.stage = stage;
            }

            public MaterialSource(ShaderStage stage, params string[] src)
            {
                this.src = string.Join("\n", src);
                this.stage = stage;
            }

            internal void Dispose()
            {
                src = null;
            }
        }

        static Dictionary<Type, Material_ShaderProgram> shaderPrograms = new Dictionary<Type, Material_ShaderProgram>();

        protected abstract MaterialSource[] MaterialSources { get; }

        private Material_ShaderProgram mat_sp;

        bool compSuccRun;

        Transform trans;

        protected Transform transform => trans;

        internal void SetTransform(Transform tr) => trans = tr;

        protected Material() { mat_sp = Get_ShaderProgram(); compSuccRun = false; }

        internal void Use()
        {
            mat_sp.Use();
        }

        internal void UpdateUniforms_Internal()
        {
            mat_sp.Use();

            if (!compSuccRun)
            {
                OnCompilationSuccess();
                compSuccRun = true;
            }

            UpdateUniforms();
        }
        
        protected void SetUniform(string name, float f1) => mat_sp.ShaderProg.SetUniform_1(mat_sp.GetUniformLocation(name), f1);
        protected void SetUniform(string name, int i1) => mat_sp.ShaderProg.SetUniform_1(mat_sp.GetUniformLocation(name), i1);
        protected void SetUniform(string name, uint ui1) => mat_sp.ShaderProg.SetUniform_1(mat_sp.GetUniformLocation(name), ui1);

        protected void SetUniform(uint loc, float f1) => mat_sp.ShaderProg.SetUniform_1(loc, f1);
        protected void SetUniform(uint loc, int i1) => mat_sp.ShaderProg.SetUniform_1(loc, i1);
        protected void SetUniform(uint loc, uint ui1) => mat_sp.ShaderProg.SetUniform_1(loc, ui1);

        protected void SetUniform(string name, int i1, int i2) => mat_sp.ShaderProg.SetUniform_2(mat_sp.GetUniformLocation(name), i1,i2);
        protected void SetUniform(string name, uint ui1, uint ui2) => mat_sp.ShaderProg.SetUniform_2(mat_sp.GetUniformLocation(name), ui1, ui2);
        protected void SetUniform(string name, float f1, float f2) => mat_sp.ShaderProg.SetUniform_2(mat_sp.GetUniformLocation(name), f1, f2);

        protected void SetUniform(uint loc, int i1, int i2) => mat_sp.ShaderProg.SetUniform_2(loc, i1, i2);
        protected void SetUniform(uint loc, uint ui1, uint ui2) => mat_sp.ShaderProg.SetUniform_2(loc, ui1, ui2);
        protected void SetUniform(uint loc, float f1, float f2) => mat_sp.ShaderProg.SetUniform_2(loc, f1, f2);


        protected void SetUniform(string name, int i1, int i2,int i3) => mat_sp.ShaderProg.SetUniform_3(mat_sp.GetUniformLocation(name), i1, i2,i3);
        protected void SetUniform(string name, uint ui1, uint ui2,uint ui3) => mat_sp.ShaderProg.SetUniform_3(mat_sp.GetUniformLocation(name), ui1, ui2,ui3);
        protected void SetUniform(string name, float f1, float f2,float f3) => mat_sp.ShaderProg.SetUniform_3(mat_sp.GetUniformLocation(name), f1, f2,f3);

        protected void SetUniform(uint loc, int i1, int i2,int i3) => mat_sp.ShaderProg.SetUniform_3(loc, i1, i2,i3);
        protected void SetUniform(uint loc, uint ui1, uint ui2,uint ui3) => mat_sp.ShaderProg.SetUniform_3(loc, ui1, ui2,ui3);
        protected void SetUniform(uint loc, float f1, float f2,float f3) => mat_sp.ShaderProg.SetUniform_3(loc, f1, f2,f3);

        protected void SetUniform(string name, int i1, int i2,int i3, int i4) => mat_sp.ShaderProg.SetUniform_4(mat_sp.GetUniformLocation(name), i1, i2,i3,i4);
        protected void SetUniform(string name, uint ui1, uint ui2,uint ui3, uint ui4) => mat_sp.ShaderProg.SetUniform_4(mat_sp.GetUniformLocation(name), ui1, ui2,ui3,ui4);
        protected void SetUniform(string name, float f1, float f2,float f3, float f4) => mat_sp.ShaderProg.SetUniform_4(mat_sp.GetUniformLocation(name), f1, f2,f3,f4);

        protected void SetUniform(uint loc, int i1, int i2, int i3, int i4) => mat_sp.ShaderProg.SetUniform_4(loc, i1, i2, i3, i4);
        protected void SetUniform(uint loc, uint ui1, uint ui2, uint ui3, uint ui4) => mat_sp.ShaderProg.SetUniform_4(loc, ui1, ui2, ui3, ui4);
        protected void SetUniform(uint loc, float f1, float f2, float f3, float f4) => mat_sp.ShaderProg.SetUniform_4(loc, f1, f2, f3, f4);

        protected void SetUniform(string name, Vector2 v) => mat_sp.ShaderProg.SetUniform(mat_sp.GetUniformLocation(name), v);
        protected void SetUniform(string name, Vector3 v) => mat_sp.ShaderProg.SetUniform(mat_sp.GetUniformLocation(name), v);
        protected void SetUniform(string name, Quaternion q) => mat_sp.ShaderProg.SetUniform(mat_sp.GetUniformLocation(name), q);

        protected void SetUniform(uint loc, Vector2 v) => mat_sp.ShaderProg.SetUniform(loc, v);
        protected void SetUniform(uint loc, Vector3 v) => mat_sp.ShaderProg.SetUniform(loc, v);
        protected void SetUniform(uint loc, Quaternion q) => mat_sp.ShaderProg.SetUniform(loc, q);

        protected void SetUniform(string name, Matrix4x4 m) => mat_sp.ShaderProg.SetUniform(mat_sp.GetUniformLocation(name), m);
        protected void SetUniform(string name, ITexture2D tex)
        {
            if (tex != null)
                SetUniform(name, tex.Bind());
        }

        protected void SetUniform(uint loc, Matrix4x4 m) => mat_sp.ShaderProg.SetUniform(loc, m);
        protected void SetUniform(uint loc, ITexture2D tex)
        {
            if (tex != null)
                SetUniform(loc, tex.Bind());
        }

        protected uint GetUniformLocation(string name)
        {
            return mat_sp.GetUniformLocation(name);
        }

        protected abstract void UpdateUniforms();
        protected abstract void OnCompilationSuccess();

        Material_ShaderProgram Get_ShaderProgram()
        {
            Material_ShaderProgram m_sp = null;
            Console.WriteLine($"Getting Material ShaderProgram for {GetType()}");
            if (!shaderPrograms.TryGetValue(GetType(),out m_sp))
            {
                Console.WriteLine("Shaderprogram not found, creating a new one");
                ShaderProgram sp = new ShaderProgram();
                MaterialSource[] sources = MaterialSources;
                if (sources == null)
                    throw new Exception("Material has no source code!");

                Shader[] shaders = new Shader[sources.Length];
                for (int i = 0; i < sources.Length; i++)
                {
                    shaders[i] = new Shader(sources[i].Stage);
                    shaders[i].SetShaderSource(sources[i].Source);
                    if (!shaders[i].Compile())
                        throw new Exception("Shader compilation failed!");
                    sp.AttachShader(shaders[i]);
                }

                if (!sp.LinkShader())
                    throw new Exception("Shaderprogram linking failed!");

                sp.DetachShaders();

                for (int i = 0; i < shaders.Length; i++)
                    shaders[i].Dispose();

                m_sp = new Material_ShaderProgram(sp);
                shaderPrograms.Add(GetType(), m_sp);
            }

            return m_sp;
        }
    }
}
