using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Shaders.Sources
{
    public sealed class ScreenQuadVertexSource : ShaderSource
    {
        public override ShaderStage Stage => ShaderStage.VERTEX;

        public override string Source => string.Join(Environment.NewLine, 
                                      "#version 330 core",
                                      "layout(location = 0) in vec3 vertPos;",
                                      "layout(location = 1) in vec2 uv;",
                                      "out vec2 fragUV;",
                                      "void main() {",
                                      "gl_Position = vec4(vertPos,1);",
                                      "fragUV = uv;",
                                      "}");
    }
}
