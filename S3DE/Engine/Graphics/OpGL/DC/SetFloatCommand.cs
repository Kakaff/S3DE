using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.DC
{
    class SetFloatCommand : IDrawCallCommand
    {
        uint uniformLocation;
        float value;

        internal SetFloatCommand(uint uniformLocation, float value)
        {
            this.uniformLocation = uniformLocation;
            this.value = value;
        }

        public void Dispose() { }
        public void OnAdd(DrawCall dc) { }

        public void Perform()
        {
            Gl.Uniform1((int)uniformLocation, value);
            OpenGL_Renderer.TestForGLErrors();
        }
    }
}
