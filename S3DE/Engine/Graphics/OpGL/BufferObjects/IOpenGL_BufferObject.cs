using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.BufferObjects
{
    internal interface IOpenGL_BufferObject
    {
        uint Identifier { get; }
        BufferTarget BufferTarget { get; }
        void Bind();
        void Unbind();
        void Dispose();
    }
}
