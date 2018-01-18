using S3DE.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_OGL_Renderer.Shaders
{
    public sealed class Sample_VertesShader_Source : ShaderSource
    {
        public Sample_VertesShader_Source()
        {
            SetStage(ShaderStage.Vertex);
            SetSource("");
        }
    }
}
