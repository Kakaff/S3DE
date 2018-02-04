using S3DE.Engine.Graphics;
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
        Texture2D texture;

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
                + "uniform mat4 view; " + '\n'
                + "uniform mat4 projection; " + '\n'
                + "uniform mat4 transform; " + '\n'
                + "out vec2 uv;" + '\n'
                + "void main() " + '\n'
                + "{ " + '\n'
                + "uv = uvs;" + '\n'
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
              + "in vec2 uv;" + '\n'
			  + "out vec4 fragColor; " + '\n'
              + "uniform sampler2D tex;" + '\n'
              + "void main() { " + '\n'
              + "fragColor = texture(tex,uv);" + '\n'
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
            CreateRendererMaterial();
            texture = createSampleTexture(new Vector2(16, 16));
        }

        Texture2D createSampleTexture(Vector2 resolution)
        {
            Texture2D tex = Texture2D.Create((int)resolution.x, (int)resolution.y);
            float xMod = 255 / resolution.x;
            float yMod = 255 / resolution.y;

            for (int x = 0; x < resolution.x; x++)
                for (int y = 0; y < resolution.y; y++)
                    tex.SetPixel(x, y, new Color((byte)(x * xMod), (byte)(y * yMod), (byte)(((x * xMod) + (y * yMod)) / 2), 255));

            tex.FilterMode = FilterMode.Nearest;
            tex.AnisotropicSamples = AnisotropicSamples.x16;
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
            return new string[] { "tex" };
        }

        protected override void UpdateUniforms()
        {
            SetTexture("tex",0,texture);
        }
    }
}
