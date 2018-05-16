using OpenGL;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.DC
{
    class SetVector3Command : IDrawCallCommand
    {
        uint uniformLocation;
        Vector3 vec;

        internal SetVector3Command(uint uniformLocation, Vector3 vec)
        {
            this.uniformLocation = uniformLocation;
            this.vec = vec;
        }
        public void Dispose() { }

        public void OnAdd(DrawCall dc) { }

        public void Perform()
        {
            Gl.Uniform3((int)uniformLocation, vec.ToArray());
            OpenGL_Renderer.TestForGLErrors();
        }
    }
}
