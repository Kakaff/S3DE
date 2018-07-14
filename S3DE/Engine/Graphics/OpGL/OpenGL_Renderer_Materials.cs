using OpenGL;
using S3DE.Engine.Graphics.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL
{
    public sealed partial class OpenGL_Renderer : Renderer
    {
        Dictionary<uint, OpenGL_ShaderProgram> shaders;

        protected override uint CreateShaderProgram()
        {
            uint pointr = Gl.CreateProgram();
            TestForGLErrors();
            OpenGL_ShaderProgram s = new OpenGL_ShaderProgram(pointr);
            shaders.Add(pointr, s);
            return pointr;
        }

        protected override void Bind_ShaderProgram(uint identifier)
        {
            if (shaders.TryGetValue(identifier, out OpenGL_ShaderProgram shader))
                shader.UseProgram();
            else
                throw new ArgumentNullException($"ShadersProgram {identifier} does not exist!");
        }

        protected override bool ShaderProgram_IsCompiled(uint identifier)
        {
            if (shaders.TryGetValue(identifier, out OpenGL_ShaderProgram shad))
                return shad.IsCompiled;
            else
                throw new ArgumentNullException($"ShaderProgram {identifier} does not exist!");
        }

        protected override void CompileShaderProgram(uint identifier)
        {
            if (shaders.TryGetValue(identifier,out OpenGL_ShaderProgram shad))
            {
                if (!shad.IsCompiled)
                    shad.Compile();
                else
                    throw new Exception($"ShaderProgram {identifier} is already compiled!");
            } else
                throw new ArgumentNullException($"ShaderProgram {identifier} does not exist!");
        }

        protected override void ShaderProgramSetSource(uint identifier, ShaderStage stage, string src)
        {
            if (shaders.TryGetValue(identifier, out OpenGL_ShaderProgram shad))
            {
                shad.SetSource(stage, src);
            }
            else
                throw new ArgumentNullException($"ShaderProgram {identifier} does not exist!");
        }
    }
}
