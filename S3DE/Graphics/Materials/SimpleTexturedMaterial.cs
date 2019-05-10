using S3DE.Graphics.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Materials
{
    public sealed class SimpleTexturedMaterial : Material
    {
        public ITexture2D Texture { get; set; }

        uint transLoc, projLoc, viewLoc, texLoc;

        protected override MaterialSource[] MaterialSources
        {
            get =>
                new MaterialSource[] {new MaterialSource(ShaderStage.VERTEX,
                                      "#version 330 core",
                                      "layout(location = 0) in vec3 vertPos;",
                                      "layout(location = 1) in vec2 uv;",
                                      "out vec2 fragUV;",
                                      "uniform mat4 transMatr;",
                                      "uniform mat4 projMatr;",
                                      "uniform mat4 viewMatr;",
                                      "void main() {",
                                      "gl_Position = (projMatr * viewMatr) *  (transMatr * vec4(vertPos,1));",
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
        }

        protected override void UpdateUniforms()
        {
            SetUniform(transLoc, transform.WorldTransformMatrix);
            SetUniform(projLoc, transform.Scene.ActiveCamera.ProjectionMatrix);
            SetUniform(viewLoc, transform.Scene.ActiveCamera.ViewMatrix);
            SetUniform(texLoc, Texture);
        }

        protected override void OnCompilationSuccess()
        {
            transLoc = GetUniformLocation("transMatr");
            projLoc = GetUniformLocation("projMatr");
            viewLoc = GetUniformLocation("viewMatr");
            texLoc = GetUniformLocation("tex");
        }
    }
}
