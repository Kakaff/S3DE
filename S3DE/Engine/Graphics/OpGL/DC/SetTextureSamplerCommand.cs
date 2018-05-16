using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.DC
{
    class SetTextureSamplerCommand : IDrawCallCommand
    {
        IOpenGL_Texture tex;
        uint uniformLocation;

        private SetTextureSamplerCommand() { }

        internal SetTextureSamplerCommand(uint uniformLocation, IOpenGL_Texture tex)
        {
            this.uniformLocation = uniformLocation;
            this.tex = tex;
        }

        public void Dispose()
        {
            tex = null;
            uniformLocation = 0;
        }

        public void Perform()
        {
            tex.Bind();
            Gl.Uniform1((int)uniformLocation, (int)tex.BoundTextureUnit);
        }

        public void OnAdd(DrawCall dc) { dc.AddTexture(tex); }
    }
}
