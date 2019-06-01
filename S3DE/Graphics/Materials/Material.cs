using S3DE.Components;
using S3DE.Graphics.Rendering;
using S3DE.Graphics.Shaders;
using S3DE.Graphics.Textures;
using S3DE.Maths;
using S3DECore.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Materials
{
    public abstract class Material
    {
        static Dictionary<Type, S3DECore.Graphics.ShaderProgram> ShaderPrograms = new Dictionary<Type, S3DECore.Graphics.ShaderProgram>();
        S3DECore.Graphics.ShaderProgram shadProg;

        public int TargetRenderPass { get; set; }

        public int ShaderProgramID => shadProg.GetInstanceID();
        Drawcall trgDC;
        Transform trgTrans;

        protected Transform transform => trgTrans;

        internal void SetTargetDrawcall(Drawcall dc) => trgDC = dc;
        internal void SetTargetTransform(Transform trans) => trgTrans = trans;

        private Material()
        {
            GetShaderProgram();
        }

        protected Material(int targetRenderPass) : this()
        {
            TargetRenderPass = targetRenderPass;
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
            if (!ShaderPrograms.TryGetValue(GetType(),out shadProg))
            {
                ShaderSource[] sources = ShaderSources;
                S3DECore.Graphics.Shader[] shaders = new S3DECore.Graphics.Shader[sources.Length];

                shadProg = new S3DECore.Graphics.ShaderProgram();

                for (int i = 0; i < sources.Length; i++)
                {
                    shaders[i] = new S3DECore.Graphics.Shader((int)sources[i].Stage);
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

        internal (UniformUpdate[],int[]) GetUniforms()
        {
            int uniformCount = shadProg.GetActiveUniformCount();
            List<UniformUpdate> uniforms = new List<UniformUpdate>();
            List<int> textures = new List<int>();
            for (int i = 0; i < uniformCount; i++)
            {
                UniformType t = shadProg.GetActiveUniformType(i);
                AddUniformToList(t);
            }

            void AddUniformToList(UniformType t)
            {
                switch (t)
                {
                    case UniformType.Matrixf4x4: uniforms.Add(new UniformUpdateMatrix4x4(uniforms.Count)); break;
                    case UniformType.TextureSampler2D:
                        {
                            uniforms.Add(new UniformUpdateTex2D(uniforms.Count, textures.Count));
                            textures.Add(0);
                            break;
                        }
                    default: throw new ArgumentException($"Unknown UniformType ({t})");
                }
            }

            return (uniforms.ToArray(),textures.ToArray());
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
            return loc;
        }
        
        protected void SetUniform(int location, Matrix4x4 matr) => trgDC.SetUniformUpdateMatrixf4(location, ref matr);
        protected void SetUniform(int location, RenderTexture2D tex) => trgDC.SetUniformUpdateTex2D(location, tex);
        

    }
}
