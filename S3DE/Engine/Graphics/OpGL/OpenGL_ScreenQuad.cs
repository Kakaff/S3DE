using OpenGL;
using S3DE;
using S3DE.Engine.Graphics;
using S3DE.Engine.Graphics.Shaders;
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
        OpenGL_Mesh quadMesh;
        ShaderProgram shader;
        internal OpenGL_ScreenQuad()
        {
            shader = Renderer.Create_ShaderProgram();
            shader.SetSource(Materials.ShaderStage.Vertex,
              "#version 400 " + '\n'
            + "layout(location = 0)in vec3 position; " + '\n'
            + "layout(location = 1)in vec2 uvs;" + '\n'
            + "out vec2 uv;" + '\n'
            + "void main()" + '\n'
            + "{ " + '\n'
            + "uv = uvs;" + '\n'
            + "gl_Position = vec4(position,1.0);" + '\n'
            + "}");

            shader.SetSource(Materials.ShaderStage.Fragment,
            "#version 400" + '\n'
          + "in vec2 uv;" + '\n'
          + "out vec4 fragColor; " + '\n'
          + "uniform sampler2D tex;" + '\n'
          + "void main() { " + '\n'
          + "fragColor = vec4(texture(tex,uv).rgb,1);" + '\n'
          + "} " + '\n');

            shader.Compile();

            quadMesh = new OpenGL_Mesh();
            quadMesh.SetData(
                new System.Numerics.Vector3[] {new System.Numerics.Vector3(-1,1,0), new System.Numerics.Vector3(1,1,0),
                               new System.Numerics.Vector3(1,-1,0),new System.Numerics.Vector3(-1,-1,0)},
                new S3DE_Vector2[] {new S3DE_Vector2(0,1),new S3DE_Vector2(1,1),
                                new S3DE_Vector2(1,0),new S3DE_Vector2(0,0)}, 
                new System.Numerics.Vector3[] { }, 
                new System.Numerics.Vector4[] {},
                new int[] {0,1,2,0,2,3},
                false);
        }

        protected override void RenderFrameToScreen(RenderTexture2D frame)
        {
            shader.Bind();
            quadMesh.Bind();
            shader.SetTextureSampler("tex",frame);
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
    }

    
}
