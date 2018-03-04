using S3DE.Engine.Graphics;
using S3DE.Engine.Graphics.Materials;
using S3DE.Engine.Graphics.Textures;
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
        Texture2D texture,normal;

        private class SimpleVertSource : MaterialSource
        {
            public SimpleVertSource()
            {
                SetStage(ShaderStage.Vertex);
                SetSource(
                  "#version 330 core " + '\n'
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
                + "gl_Position = (projection * view) * vec4(frag.pos,1.0); " + '\n'
                + "}");
            }
        }

        private class SimpleFragSource : MaterialSource
        {
            public SimpleFragSource()
            {
                SetStage(ShaderStage.Fragment);
                SetSource(
                "#version 330 core" + '\n'
              + "layout(location = 0) out vec3 gFragColor; " + '\n'
              + "layout(location = 1) out vec4 gNormalSpec;" + '\n'
              + "layout(location = 2) out vec3 gPosition;" + '\n'

              + "in Frag { " + '\n'
              + "vec3 pos;" + '\n'
              + "vec2 uv;" + '\n'
              + "mat3 TBN;" + '\n'
              + "} frag;" + '\n'

              + "uniform sampler2D tex;" + '\n'
              + "uniform sampler2D normalMap;" + '\n'
              + "void main() { " + '\n'
              + "gFragColor = vec3(texture(tex,frag.uv).rgb);" + '\n'
              + "gPosition = frag.pos;" + '\n'
              + "gNormalSpec = vec4(frag.TBN * (texture(normalMap,frag.uv).rgb * 2 - 1),0);" + '\n'
              + "} " + '\n');
            }
        }

        public SimpleMaterial() : base()
        {
            UsesProjectionMatrix = true;
            UsesViewMatrix = true;
            UsesTransformMatrix = true;
            UsesRotationMatrix = true;
            normal = ImageLoader.LoadFromFile(Environment.CurrentDirectory + @"\brickwall_normal.jpg");
            texture = ImageLoader.LoadFromFile(Environment.CurrentDirectory + @"\brickwall.jpg");
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

        protected override MaterialSource GetSource(ShaderStage stage)
        {
            switch (stage)
            {
                case ShaderStage.Vertex:
                    {
                        return new SimpleVertSource();
                    }
                case ShaderStage.Fragment:
                    {
                        return new SimpleFragSource();
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
