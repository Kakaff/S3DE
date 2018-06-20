using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.BufferObjects
{
    internal struct OpenGL_ElementArrayBufferObject : IOpenGL_BufferObject
    {
        static OpenGL_ElementArrayBufferObject BoundBuffer;

        uint id;

        public uint Identifier => id;

        public BufferTarget BufferTarget => BufferTarget.ElementArrayBuffer;

        internal OpenGL_ElementArrayBufferObject(uint id) => this.id = id;

        public void Bind()
        {
            if (BoundBuffer.Identifier != this.Identifier)
            {
                Gl.BindBuffer(BufferTarget, Identifier);
                BoundBuffer = this;
            }
        }

        public void Dispose()
        {

        }

        public void Unbind()
        {

        }
    }
}
