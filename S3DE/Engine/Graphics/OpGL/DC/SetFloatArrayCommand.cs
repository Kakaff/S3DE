using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.DC
{

    class SetFloatArrayCommand : IDrawCallCommand
    {
        float[] data;
        uint uniformLocation;

        internal SetFloatArrayCommand(uint uniformLocation, float[] data)
        {
            this.uniformLocation = uniformLocation;
            this.data = data;
        }

        public void Dispose()
        {
            data = null;
            uniformLocation = 0;
        }

        public void Perform()
        {
            Gl.Uniform1((int)uniformLocation, data);
            OpenGL_Renderer.TestForGLErrors();
        }

        public void OnAdd(DrawCall dc) { }
    }
}
