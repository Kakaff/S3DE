using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_OGL_Renderer
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

        internal void Bind() => Gl.BindVertexArray(id);

        internal void Unbind() => Gl.BindVertexArray(0);

    }
}
