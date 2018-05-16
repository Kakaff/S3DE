using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.DC
{
    class SetIntegerCommand : IDrawCallCommand
    {
        uint uniformLocation;
        int value;

        internal SetIntegerCommand(uint uniformLocation, int value)
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
