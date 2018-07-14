using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.BufferObjects
{
    
    internal static class OpenGL_BufferObject
    {
        internal static IOpenGL_BufferObject CreateBuffer(BufferTarget bufferTarget)
        {
            switch (bufferTarget)
            {
                case BufferTarget.ArrayBuffer: { return new OpenGL_ArrayBufferObject(Gl.GenBuffer());}
                case BufferTarget.ElementArrayBuffer: { return new OpenGL_ElementArrayBufferObject(Gl.GenBuffer());}

                default: throw new NotSupportedException("");
            }
        }
    }
}
