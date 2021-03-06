﻿using S3DE.Graphics.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Screen
{
    /// <summary>
    /// A simple material that draws a texture to the screen
    /// </summary>
    public sealed class DefaultScreenQuadMaterial : Material
    {
        static DefaultScreenQuadMaterial instance;
        public static DefaultScreenQuadMaterial Instance { get { if (instance == null) instance = new DefaultScreenQuadMaterial(); return instance; } }

        private DefaultScreenQuadMaterial() : base()
        {

        }

        public IRenderTexture Tex;

        uint texLoc;

        protected override MaterialSource[] MaterialSources =>
            new MaterialSource[] {
                    new MaterialSource(ShaderStage.VERTEX,
                                      "#version 330 core",
                                      "layout(location = 0) in vec3 vertPos;",
                                      "layout(location = 1) in vec2 uv;",
                                      "out vec2 fragUV;",
                                      "void main() {",
                                      "gl_Position = vec4(vertPos,1);",
                                      "fragUV = uv;",
                                      "}"),

                    new MaterialSource(ShaderStage.FRAGMENT,
                                      "#version 330 core",
                                      "in vec2 fragUV;",
                                      "out vec3 col;",
                                      "uniform sampler2D tex;",
                                      "void main() {",
                                      "col = texture(tex,fragUV).rgb;",
                                      "}")};

        protected override void UpdateUniforms()
        {
            SetUniform(texLoc, Tex.Bind());
        }

        protected override void OnCompilationSuccess()
        {
            texLoc = GetUniformLocation("tex");
        }
    }
}
