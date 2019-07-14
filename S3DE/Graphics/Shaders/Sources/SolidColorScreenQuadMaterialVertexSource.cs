using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Shaders.Sources
{
    public sealed class SolidColorScreenQuadMaterialVertexSource : ShaderSource
    {
        public override ShaderStage Stage => ShaderStage.Vertex;

        public override string Source => string.Join(Environment.NewLine,
                                      "#version 330 core",
                                      "layout(location = 0) in vec3 vertPos;",
                                      "void main() {",
                                      "gl_Position = vec4(vertPos,1);",
                                      "}");
    }
}
