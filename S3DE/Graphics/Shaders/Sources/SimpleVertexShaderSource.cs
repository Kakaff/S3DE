﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Shaders.Sources
{
    public sealed class SimpleVertexShaderSource : ShaderSource
    {
        public override ShaderStage Stage => ShaderStage.VERTEX;

        public override string Source => string.Join(Environment.NewLine,
                                  "#version 330 core",
                                  "layout(location = 0) in vec3 vertPos;",
                                  "uniform mat4 transMatr;",
                                  "uniform mat4 projMatr;",
                                  "uniform mat4 viewMatr;",
                                  "void main() {",
                                  "gl_Position = (projMatr * viewMatr) *  (transMatr * vec4(vertPos,1));",
                                  "}");
    }
}