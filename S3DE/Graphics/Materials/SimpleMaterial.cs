using S3DE.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Materials
{
    public sealed class SimpleMaterial : Material
    {
        public Color Color { get; set; }

        uint proj, trans, view, col;
        public SimpleMaterial() : base()
        {
            Color = new Color(255, 255, 255, 255);
        }

        protected override MaterialSource[] MaterialSources => new MaterialSource[]
        {
            new MaterialSource(ShaderStage.VERTEX,
                                "#version 330 core",
                                "layout(location = 0) in vec3 vert;",
                                "uniform mat4 trans;",
                                "uniform mat4 proj;",
                                "uniform mat4 view;",

                                "void main() {",
                                "gl_Position = (proj * view) *  (trans * vec4(vert,1));",
                                "}"
                                ),

            new MaterialSource(ShaderStage.FRAGMENT,
                               "#version 330 core",
                               "out vec3 color;",

                               "uniform vec3 col;",
                               "void main() {",
                               "color = col.rgb;",
                               "}"
                               )
        };

        protected override void UpdateUniforms()
        {
            SetUniform(proj, transform.Scene.ActiveCamera.ProjectionMatrix);
            SetUniform(view, transform.Scene.ActiveCamera.ViewMatrix);
            SetUniform(trans, transform.WorldTransformMatrix);
            SetUniform(col, Color);
        }

        protected override void OnCompilationSuccess()
        {
            col = GetUniformLocation("col");
            proj = GetUniformLocation("proj");
            trans = GetUniformLocation("trans");
            view = GetUniformLocation("view");
        }
    }
}
