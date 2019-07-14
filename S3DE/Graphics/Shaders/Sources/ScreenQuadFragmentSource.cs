using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Shaders.Sources
{
    public sealed class ScreenQuadFragmentSource : ShaderSource
    {
        public override ShaderStage Stage => ShaderStage.Fragment;

        public override string Source => string.Join(Environment.NewLine, 
                                      "#version 330 core",
                                      "in vec2 fragUV;",
                                      "out vec3 col;",
                                      "uniform sampler2D tex;",
                                      "void main() {",
                                      "col = texture(tex,fragUV).rgb;",
                                      "}");
    }
}
