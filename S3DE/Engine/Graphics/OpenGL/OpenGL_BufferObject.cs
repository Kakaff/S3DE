using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL
{
    internal struct OpenGL_BufferObject
    {
        uint id;
        BufferTarget bufferTarget;

        internal uint ID => id;

        internal OpenGL_BufferObject(uint name,BufferTarget bufferTarget)
        {
            id = name;
            this.bufferTarget = bufferTarget;
        }

        internal void Bind()
        {
            Gl.BindBuffer(bufferTarget, id);
            OpenGL_Renderer.TestForGLErrors();
        }

        internal void Unbind() => Gl.BindBuffer(bufferTarget, 0);   
        internal void Dispose() => Gl.DeleteBuffers(id);
    }
}
