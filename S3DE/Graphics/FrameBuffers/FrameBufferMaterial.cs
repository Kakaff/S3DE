using S3DE.Graphics.Shaders;
using S3DE.Graphics.Textures;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.FrameBuffers
{
    public abstract class FrameBufferMaterial
    {
        FrameBufferMat mat;

        private class FrameBufferMat : Material
        {
            public IRenderTexture Tex;

            protected override MaterialSource[] MaterialSources => 
                new MaterialSource[] {
                    new MaterialSource(ShaderStage.VERTEX, 
                                      "#version 330 core",
                                      "layout(location = 0) in vec3 vertPos;",
                                      "layout(location = 1) in vec2 uv;",
                                      "out vec2 fragUV;",
                                      "void main() {",
                                      "gl_Position = vec4(vertPos,1);",
                                      "fragUV = uv;",
                                      "}"),

                    new MaterialSource(ShaderStage.FRAGMENT,
                                      "#version 330 core",
                                      "in vec2 fragUV;",
                                      "out vec3 col;",
                                      "uniform sampler2D tex;",
                                      "void main() {",
                                      "col = texture(tex,fragUV).rgb;",
                                      "}")};

            protected override void UpdateUniforms()
            {
                SetUniform("tex", Tex.Bind());
            }
        }

        protected FrameBufferMaterial()
        {

            mat = new FrameBufferMat();
            
        }

        protected void DrawTexture(Vector2 offset, Vector2 scale, IRenderTexture tex)
        {
            mat.Tex = tex;
            mat.UpdateUniforms_Internal();
            
            ScreenQuad.Draw();
        }

        internal abstract void PresentFrame(FrameBuffer fb);


        private static class ScreenQuad
        {
            static Mesh m;
            
            static void Create()
            {
                if (m == null)
                {
                    Console.WriteLine("Creating new SCREENQUAD mesh!");
                    m = new Mesh();
                    m.SetVertexAttribute(0, 3, GLType.FLOAT, false, 20, 0);
                    m.SetVertexAttribute(1, 2, GLType.FLOAT, false, 20, 12);
                    m.EnableVertexAttribute(0);
                    m.EnableVertexAttribute(1);

                    m.Vertices = new Vector3[] { new Vector3(-1, -1), new Vector3(-1, 1), new Vector3(1, 1), new Vector3(1, -1) };
                    m.UVs = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
                    m.Indicies = new ushort[] { 0, 1, 2, 2, 3, 0 };
                    m.Apply();
                }
            }
            
            public static void Draw()
            {
                if (m == null)
                    Create();
                
                m.Draw();
            }
        }
    }
}
