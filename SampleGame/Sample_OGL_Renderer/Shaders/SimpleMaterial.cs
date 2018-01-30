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

        private class SimpleVertSource : ShaderSource
        {
            public SimpleVertSource()
            {
                SetStage(ShaderStage.Vertex);
                SetSource(
                  "#version 400 " + '\n'
				+ "layout(location = 0)in vec3 position; " + '\n'
                + "uniform mat4 view; " + '\n'
                + "uniform mat4 projection; " + '\n'
                + "uniform mat4 transform; " + '\n'
                + "void main() " + '\n'
                + "{ " + '\n'
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
			  + "out vec4 fragColor; " + '\n'
              + "void main() { " + '\n'
              + "fragColor = vec4(1.0f,1.0,1.0f,1.0f);" + '\n'
              + "} " + '\n');
            }
        }

        public SimpleMaterial() : base()
        {
            VertexSource = new SimpleVertSource();
            FragmentSource = new SimpleFragSource();

            UsesProjectionMatrix = true;
            UsesViewMatrix = true;
            UsesTransformMatrix = true;
        }

        protected override ShaderSource GetSource(ShaderStage stage)
        {
            if (VertexSource == null)
                VertexSource = new SimpleVertSource();

            if (FragmentSource == null)
                FragmentSource = new SimpleFragSource();

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

        protected override void UpdateUniforms()
        {
            //throw new NotImplementedException();
        }
    }
}
