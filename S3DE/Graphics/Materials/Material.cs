using S3DE.Components;
using S3DE.Graphics.Shaders;
using S3DECore.Graphics.Shaders;
using S3DECore.Graphics.Textures;
using S3DECore.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Materials
{
    public abstract class Material
    {
        static Dictionary<Type, ShaderProgram> ShaderPrograms = new Dictionary<Type, ShaderProgram>();
        ShaderProgram shadProg;
        
        public int ShaderProgramID => shadProg.GetInstanceID();
        Transform trgTrans;

        protected Transform transform => trgTrans;

        internal void SetTargetTransform(Transform trans) => trgTrans = trans;

        protected Material()
        {
            GetShaderProgram();
        }
        

        protected abstract ShaderSource[] ShaderSources { get; }

        internal void UpdateUniforms_Internal()
        {
            UpdateUniforms();
        }

        internal void UseMaterial()
        {
            if (shadProg == null)
                GetShaderProgram();

            shadProg.Use();

        }

        void GetShaderProgram()
        {
            if (!ShaderPrograms.TryGetValue(GetType(), out shadProg))
            {
                ShaderSource[] sources = ShaderSources;
                Shader[] shaders = new Shader[sources.Length];

                shadProg = new ShaderProgram();

                for (int i = 0; i < sources.Length; i++)
                {
                    shaders[i] = new Shader((int)sources[i].Stage);
                    unsafe
                    {
                        fixed (byte* s = &Encoding.ASCII.GetBytes(sources[i].Source)[0])
                            shaders[i].SetSource((sbyte*)s);
                    }

                    if (!shaders[i].Compile())
                        throw new Exception($"Error compiling shader! {Environment.NewLine}" +
                                            $"Shader Source: {Environment.NewLine}" +
                                            $"{sources[i].Source}");

                    shadProg.AttachShader(shaders[i]);
                }

                if (!shadProg.Link())
                    throw new Exception("Error linking shaderprogram!");
            }

            OnCompilationSuccess();
        }

        protected abstract void OnCompilationSuccess();
        protected abstract void UpdateUniforms();


        protected int GetUniformLocation(string uniformName)
        {
            int loc = -1;
            unsafe
            {
                fixed (byte* n = Encoding.ASCII.GetBytes(uniformName))
                    loc = shadProg.GetUniformLocation((sbyte*)n);
            }
            if (loc == -1)
                throw new Exception("Unable to find uniform");
            return loc;
        }

        protected void SetUniform(int location, Matrix4x4 matr) => ShaderProgram.SetUniformMatrixf4v((uint)location, 1, true, matr);
        protected void SetUniform(int location, Vector3 v) => ShaderProgram.SetUniform3f((uint)location, v);
        protected void SetUniform(int location, RenderTexture2D tex) => ShaderProgram.SetUniform1i((uint)location, (int)tex.Bind());
    }
}
