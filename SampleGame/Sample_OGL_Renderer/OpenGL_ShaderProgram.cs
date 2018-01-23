using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_OGL_Renderer
{
    internal sealed class OpenGL_ShaderProgram
    {
        OpenGL_Shader vertexShader;
        OpenGL_Shader fragmentShader;

        uint pointer;

        //Create a collection similar to the shaders later. But take in a material instead.

        internal OpenGL_ShaderProgram(OpenGL_Shader VertexShader, OpenGL_Shader FragmentShader)
        {
            this.vertexShader = VertexShader;
            this.fragmentShader = FragmentShader;

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

            Gl.BindAttribLocation(pointer, 0, "pos");
            //Gl.BindAttribLocation(pointer, 1, "uv");
            //Gl.BindAttribLocation(pointer, 2, "normal");

            Gl.LinkProgram(pointer);
        }

        internal void UseProgram() => Gl.UseProgram(pointer);

        internal void Dispose()
        {
            //Free all shaders.
        }
    }
}
