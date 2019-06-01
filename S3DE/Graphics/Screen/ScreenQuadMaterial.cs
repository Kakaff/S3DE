using S3DE.Graphics.Shaders;
using S3DE.Graphics.Textures;
using S3DECore.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Screen
{
    //Pretty much a normal Material except it doesn't use drawcalls or transforms.
    public abstract class ScreenQuadMaterial
    {
        static Dictionary<Type, ShaderProgram> shaderPrograms = new Dictionary<Type, ShaderProgram>();

        UniformUpdate[] uniformUpdates;
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
            for (int i = 0; i < uniformUpdates.Length; i++)
                uniformUpdates[i].Perform();
        }

        void GetShaderProgram()
        {
            if (!shaderPrograms.TryGetValue(GetType(), out shadProg))
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

            GetUniforms();
            OnCompilationSuccess();
        }

        internal void GetUniforms()
        {
            int uniformCount = shadProg.GetActiveUniformCount();
            List<UniformUpdate> uniforms = new List<UniformUpdate>();
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
                            uniforms.Add(new UniformUpdateTex2D(uniforms.Count, 0));
                            break;
                        }
                    default: throw new ArgumentException($"Unknown UniformType ({t})");
                }
            }

            uniformUpdates = uniforms.ToArray();
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
            UniformUpdate uu = uniformUpdates[location];
            if (uu != null && uu.UniformType == UniformType.TextureSampler2D)
            {
                UniformUpdateTex2D uut2d = uu as UniformUpdateTex2D;
                uut2d.Texture = tex;
            }
            else if (uu == null)
                ThrowNullUniformException(location);
            else if (uu.UniformType != UniformType.TextureSampler2D)
                ThrowInvalidValueException(uu, location, UniformType.TextureSampler2D);
        }



        static void ThrowNullUniformException(int location)
        {
            throw new NullReferenceException($"Uniform {location} does not exist!");
        }

        static void ThrowInvalidValueException(UniformUpdate uu, int location, UniformType ut)
        {
            throw new InvalidOperationException($"Uniform {location} expects a {Enum.GetName(typeof(UniformType), uu.UniformType)} but received a {Enum.GetName(typeof(UniformType), ut)}");
        }

    }
}
