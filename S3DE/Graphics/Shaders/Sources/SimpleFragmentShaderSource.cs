using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Shaders.Sources
{
    public sealed class SimpleFragmentShadeSource : ShaderSource
    {
        public override ShaderStage Stage => ShaderStage.Fragment;

        public override string Source => string.Join(Environment.NewLine,
                                        "#version 330 core",
                                        "out vec3 col;",
                                        "uniform vec3 trgCol;",
                                        "void main() {",
                                        "col = trgCol;",
                                        "}");
    }
}
