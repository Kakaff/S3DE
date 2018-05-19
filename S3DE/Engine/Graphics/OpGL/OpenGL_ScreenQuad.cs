﻿using OpenGL;
using S3DE;
using S3DE.Engine.Graphics;
using S3DE.Engine.Graphics.Textures;
using S3DE.Engine.IO;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL
{
    internal class OpenGL_ScreenQuad : ScreenQuad
    {
        ScreenQuadShader shader;
        OpenGL_Mesh quadMesh;
        
        internal OpenGL_ScreenQuad()
        {
            shader = new ScreenQuadShader();

            quadMesh = new OpenGL_Mesh();
            quadMesh.SetData(
                new Vector3[] {new Vector3(-1,1,0), new Vector3(1,1,0),
                               new Vector3(1,-1,0),new Vector3(-1,-1,0)},
                new Vector2[] {new Vector2(0,1),new Vector2(1,1),
                                new Vector2(1,0),new Vector2(0,0)}, 
                new Vector3[] { }, 
                new Vector4[] {},
                new int[] {0,1,2,0,2,3},
                false);
        }

        protected override void RenderFrameToScreen(RenderTexture2D frame)
        {
            shader.Use();
            quadMesh.Bind();
            shader.SetFrameTexture((int)frame.Bind());
            Gl.DrawElements(PrimitiveType.Triangles, quadMesh.Indicies, DrawElementsType.UnsignedShort, IntPtr.Zero);
            OpenGL_Renderer.TestForGLErrors();
        }

        protected override void Render()
        {
            quadMesh.Bind();
            Gl.DrawElements(PrimitiveType.Triangles, quadMesh.Indicies, DrawElementsType.UnsignedShort, IntPtr.Zero);
            OpenGL_Renderer.TestForGLErrors();
        }

        protected override void BindMesh() => quadMesh.Bind();
        protected override void UnbindMesh() => quadMesh.Unbind();

        private sealed class ScreenQuadShader
        {
            OpenGL_ShaderProgram shaderProgram;
            OpenGL_Shader vertShader, fragShader;
            
            public ScreenQuadShader()
            {
                vertShader = OpenGL_Shader.Create(ShaderType.VertexShader,
                 "#version 400 " + '\n'
                + "layout(location = 0)in vec3 position; " + '\n'
                + "layout(location = 1)in vec2 uvs;" + '\n'
                + "out vec2 uv;" + '\n'
                + "void main()" + '\n'
                + "{ " + '\n'
                + "uv = uvs;" + '\n'
                + "gl_Position = vec4(position,1.0);" + '\n'
                + "}");

                fragShader = OpenGL_Shader.Create(ShaderType.FragmentShader,
                "#version 400" + '\n'
              + "in vec2 uv;" + '\n'
              + "out vec4 fragColor; " + '\n'
              + "uniform sampler2D tex;" + '\n'
              + "void main() { " + '\n'
              + "fragColor = vec4(texture(tex,uv).rgb,1);" + '\n'
              + "} " + '\n');

                shaderProgram = new OpenGL_ShaderProgram(vertShader, fragShader);
                shaderProgram.Compile();
            }

            public void Use() => shaderProgram.UseProgram();

            public void SetFrameTexture(int textureUnit) => Gl.Uniform1(0, textureUnit);
        }
    }

    
}