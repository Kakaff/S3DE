using S3DE.Engine.Graphics;
using S3DE.Engine.IO;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_OGL_Renderer.Shaders
{
    public class SimpleMaterial : Material
    {
        ShaderSource VertexSource, FragmentSource;
        Texture2D texture,normal;

        private class SimpleVertSource : ShaderSource
        {
            public SimpleVertSource()
            {
                SetStage(ShaderStage.Vertex);
                SetSource(
                  "#version 400 " + '\n'
                + "layout(location = 0)in vec3 position; " + '\n'
                + "layout(location = 1)in vec2 uvs;" + '\n'
                + "layout(location = 2)in vec3 normal;" + '\n'
                + "layout(location = 3)in vec3 tangent;" + '\n'
                + "layout(location = 4)in vec3 bitangent;" + '\n'
                + "uniform mat4 view; " + '\n'
                + "uniform mat4 projection; " + '\n'
                + "uniform mat4 transform; " + '\n'
                + "uniform mat4 rotation; " + '\n'
                + "out Frag { " + '\n'
                + "vec3 pos;" + '\n'
                + "vec2 uv;" + '\n'
                + "mat3 TBN;" + '\n'
                + "} frag;" + '\n'

                + "void main() " + '\n'
                + "{ " + '\n'
                + "frag.pos = (transform * vec4(position,1.0)).xyz;" + '\n'
                + "frag.uv = uvs;" + '\n'
                + "frag.TBN = mat3(rotation) * mat3(tangent,bitangent,normal);" + '\n'
                + "gl_Position = (projection * view) * transform * vec4(position,1.0); " + '\n'
                + "}");
            }
        }

        private class SimpleFragSource : ShaderSource
        {
            public SimpleFragSource()
            {
                SetStage(ShaderStage.Fragment);
                SetSource(
                "#version 400" + '\n'
              + "layout(location = 0) out vec4 gFragColor; " + '\n'
              + "layout(location = 1) out vec3 gNormal;" + '\n'
              + "layout(location = 3) out vec3 gPosition;" + '\n'

              + "in Frag { " + '\n'
              + "vec3 pos;" + '\n'
              + "vec2 uv;" + '\n'
              + "mat3 TBN;" + '\n'
              + "} frag;" + '\n'

              + "uniform sampler2D tex;" + '\n'
              + "uniform sampler2D normalMap;" + '\n'
              + "void main() { " + '\n'
              + "gFragColor = texture(tex,frag.uv);" + '\n'
              + "gPosition = frag.pos;" + '\n'
              + "gNormal = frag.TBN * (texture(normalMap,frag.uv).rgb * 2 - 1);" + '\n'
              + "} " + '\n');
            }
        }

        public SimpleMaterial()
        {
            VertexSource = new SimpleVertSource();
            FragmentSource = new SimpleFragSource();

            UsesProjectionMatrix = true;
            UsesViewMatrix = true;
            UsesTransformMatrix = true;
            UsesRotationMatrix = true;
            CreateRendererMaterial();
            normal = ImageLoader.LoadFromFile(@"C:\Users\Kakaf\source\repos\S3DE\SampleGame\bin\Debug\brickwall_normal.jpg");
            texture = createSampleTexture(new Vector2(8, 8));
        }

        Texture2D createSampleTexture(Vector2 resolution)
        {
            Texture2D tex = Texture2D.Create((int)resolution.x, (int)resolution.y);
            float xMod = 255 / resolution.x;
            float yMod = 255 / resolution.y;

            for (int x = 0; x < resolution.x; x++)
                for (int y = 0; y < resolution.y; y++)
                    tex.SetPixel(x, y, new Color((byte)(x * xMod), (byte)(y * yMod), (byte)(((x * xMod) + (y * yMod)) / 2), 255));

            tex.FilterMode = FilterMode.Trilinear;
            tex.AnisotropicSamples = AnisotropicSamples.x16;
            tex.CalculateMipMapCount();
            Console.WriteLine($"Target mipmap count: " + tex.MipMapLevels);
            tex.Apply();
            return tex;
        }

        protected override ShaderSource GetSource(ShaderStage stage)
        {
            switch (stage)
            {
                case ShaderStage.Vertex:
                    {
                        return VertexSource;
                    }
                case ShaderStage.Fragment:
                    {
                        return FragmentSource;
                    }
                default:
                    {
                        throw new NotSupportedException();
                    }
            }
        }

        protected override string[] GetUniforms()
        {
            return new string[] {"tex", "normalMap"};
        }

        protected override void UpdateUniforms()
        {
            SetTexture("tex",0,texture);
            SetTexture("normalMap", 1, normal);
        }
    }
}
