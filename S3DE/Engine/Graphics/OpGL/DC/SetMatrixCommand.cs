using OpenGL;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.DC
{
    class SetMatrix4x4Command : IDrawCallCommand
    {
        Matrix4x4 matr;
        uint uniformLocation;

        internal SetMatrix4x4Command(uint uniformLocation, Matrix4x4 matr)
        {
            this.uniformLocation = uniformLocation;
            this.matr = matr;
        }

        public void Dispose()
        {
            matr = null;
        }

        public void OnAdd(DrawCall dc) { }

        public void Perform()
        {
            Gl.UniformMatrix4((int)uniformLocation, false, matr.ToArray());
            OpenGL_Renderer.TestForGLErrors();
        }
    }
}
