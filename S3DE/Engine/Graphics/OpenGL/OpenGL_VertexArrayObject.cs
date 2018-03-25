using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL
{
    internal struct OpenGL_VertexArrayObject
    {
        uint id;

        public OpenGL_VertexArrayObject(uint name) => id = name;

        internal void EnableAttribute(uint attribute) => Gl.EnableVertexAttribArray(attribute);

        internal void DisableAttribute(uint attribute) => Gl.DisableVertexAttribArray(attribute);

        internal void SetAttributePointer(uint location,int size, VertexAttribType attribType,bool normalized,int stride,int offset)
        {
            Gl.VertexAttribPointer(location, size, attribType, normalized, stride, (IntPtr)offset);
        }

        internal void Bind()
        {
            Gl.BindVertexArray(id);
            OpenGL_Renderer.TestForGLErrors();
        }

        internal void Unbind() => Gl.BindVertexArray(0);

    }
}
