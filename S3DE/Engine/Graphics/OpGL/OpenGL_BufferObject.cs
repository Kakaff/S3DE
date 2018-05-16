using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL
{
    internal interface IOpenGL_BufferObject
    {
        uint Identifier {get;}
        BufferTarget BufferTarget {get;}
        void Bind();
        void Unbind();
        void Dispose();
    }

    internal struct OpenGL_ArrayBuffer : IOpenGL_BufferObject
    {
        static OpenGL_ArrayBuffer BoundBuffer;

        uint id;
        
        public uint Identifier => id;

        public BufferTarget BufferTarget => BufferTarget.ArrayBuffer;

        internal OpenGL_ArrayBuffer(uint id) => this.id = id;

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

    internal struct OpenGL_ElementArrayBuffer : IOpenGL_BufferObject
    {
        static OpenGL_ElementArrayBuffer BoundBuffer;

        uint id;

        public uint Identifier => id;

        public BufferTarget BufferTarget => BufferTarget.ElementArrayBuffer;

        internal OpenGL_ElementArrayBuffer(uint id) => this.id = id;

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

    internal static class OpenGL_BufferObject
    {
        internal static IOpenGL_BufferObject CreateBuffer(BufferTarget bufferTarget)
        {
            switch (bufferTarget)
            {
                case BufferTarget.ArrayBuffer: { return new OpenGL_ArrayBuffer(Gl.GenBuffer());}
                case BufferTarget.ElementArrayBuffer: { return new OpenGL_ElementArrayBuffer(Gl.GenBuffer());}

                default: throw new NotSupportedException("");
            }
        }
    }

    /*
    internal struct OpenGL_BufferObject
    {
        uint id;
        BufferTarget bufferTarget;

        static Dictionary<BufferTarget, OpenGL_BufferObject> boundBuffers = new Dictionary<BufferTarget, OpenGL_BufferObject>();
        internal uint ID => id;

        internal OpenGL_BufferObject(uint name,BufferTarget bufferTarget)
        {
            id = name;
            this.bufferTarget = bufferTarget;
        }

        internal void Bind()
        {
            if (boundBuffers.TryGetValue(bufferTarget, out OpenGL_BufferObject bBuffer))
            {
                if (bBuffer.id != this.id)
                {
                    boundBuffers.Remove(bufferTarget);
                    boundBuffers.Add(bufferTarget, this);
                    Gl.BindBuffer(bufferTarget, id);
                    OpenGL_Renderer.TestForGLErrors();
                }
            } else
            {
                boundBuffers.Add(bufferTarget, this);
                Gl.BindBuffer(bufferTarget, id);
                OpenGL_Renderer.TestForGLErrors();
            }
        }

        internal void Unbind() => Gl.BindBuffer(bufferTarget, 0);   
        internal void Dispose() => Gl.DeleteBuffers(id);
    }
    */
}
