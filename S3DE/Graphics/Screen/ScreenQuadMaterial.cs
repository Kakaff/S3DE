using S3DE.Graphics.Shaders;
using S3DECore.Graphics.Shaders;
using S3DECore.Graphics.Textures;
using System;
using System.Collections.Generic;
using System.Text;

namespace S3DE.Graphics.Screen
{
    //Pretty much a normal Material except it doesn't use drawcalls or transforms.
    public abstract class ScreenQuadMaterial
    {
        static Dictionary<Type, ShaderProgram> shaderPrograms = new Dictionary<Type, ShaderProgram>();
        
        ShaderProgram shadProg;

        protected abstract ShaderSource[] ShaderSources { get; }

        protected abstract void UpdateUniforms();
        protected abstract void OnCompilationSuccess();

        internal void Update()
        {
            if (shadProg == null)
                GetShaderProgram();

            shadProg.Use();
            UpdateUniforms();
        }

        void GetShaderProgram()
        {
            if (!shaderPrograms.TryGetValue(GetType(), out shadProg))
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

        
        protected int GetUniformLocation(string uniformName)
        {
            int loc = -1;
            unsafe
            {
                fixed (byte* n = Encoding.ASCII.GetBytes(uniformName))
                    loc = shadProg.GetUniformLocation((sbyte*)n);
            }
            return loc;
        }

        public void SetUniform(int location, RenderTexture2D tex)
        {
            ShaderProgram.SetUniform1i((uint)location, (int)tex.Bind());
        }

    }
}
