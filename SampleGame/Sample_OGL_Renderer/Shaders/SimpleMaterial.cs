using S3DE.Engine.Graphics;
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
            texture = Texture2D.Create(2, 2);
            texture.SetPixel(0, 0, Color.Blue);
            texture.SetPixel(1, 0, Color.Green);
            texture.SetPixel(0, 1, Color.Red);
            texture.SetPixel(1, 1, Color.White);
            texture.Apply();
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
