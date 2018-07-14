using S3DE.Engine.Graphics.Materials;
using S3DE.Engine.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public abstract partial class Renderer
    {
        uint boundShaderProgram;

        public static uint BoundShaderProgram => ActiveRenderer.boundShaderProgram;

        public static ShaderProgram Create_ShaderProgram() => new ShaderProgram(ActiveRenderer.CreateShaderProgram());

        public static void Compile_ShaderProgram(ShaderProgram shader) => ActiveRenderer.CompileShaderProgram(shader.Identifier);
        public static void ShaderProgram_SetSource(ShaderProgram shader,ShaderStage stage,params string[] src)
        {
            ActiveRenderer.ShaderProgramSetSource(shader.Identifier,stage, String.Join("\n",src));
        }

        public static bool Check_ShaderProgram_IsCompiled(ShaderProgram shader) => ActiveRenderer.ShaderProgram_IsCompiled(shader.Identifier);

        public static void ShaderProgram_Bind(ShaderProgram shader) {
            if (BoundShaderProgram == shader.Identifier)
                return;

            ActiveRenderer.Bind_ShaderProgram(shader.Identifier);
            ActiveRenderer.boundShaderProgram = shader.Identifier;
        }

        protected abstract void CompileShaderProgram(uint identifier);
        protected abstract bool ShaderProgram_IsCompiled(uint identifier);
        protected abstract void Bind_ShaderProgram(uint identifier);
        protected abstract uint CreateShaderProgram();
        protected abstract void ShaderProgramSetSource(uint identifier,ShaderStage stage, string src);
    }
}
