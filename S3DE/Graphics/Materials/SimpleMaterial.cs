using S3DE.Graphics.Shaders;
using S3DE.Graphics.Shaders.Sources;
using S3DECore.Graphics;
using S3DECore.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Materials
{
    public sealed class SimpleMaterial : Material
    {
        int transLoc, projLoc, viewLoc, trgColLoc;

        public Color Color { get; set; }

        protected override ShaderSource[] ShaderSources => new ShaderSource[]
            {new SimpleVertexShaderSource(), new SimpleFragmentShadeSource()};

        public SimpleMaterial() : base() {
            Color = new Color(255, 255, 255, 0);
            SetTargetRenderpass<DeferredRenderpass>();
        }

        protected override void OnCompilationSuccess()
        {
            trgColLoc = GetUniformLocation("trgCol");
            transLoc = GetUniformLocation("transMatr");
            projLoc = GetUniformLocation("projMatr");
            viewLoc = GetUniformLocation("viewMatr");
        }

        protected override void UpdateUniforms()
        {
            SetUniform(trgColLoc, new Vector3(Color.Red / 255f, Color.Green / 255f, Color.Blue / 255f));
            SetUniform(projLoc, transform.Scene.ActiveCamera.ProjectionMatrix);
            SetUniform(viewLoc, transform.Scene.ActiveCamera.ViewMatrix);
            SetUniform(transLoc, transform.WorldMatrix);
        }
    }
}
