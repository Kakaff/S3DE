using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL
{
    internal class OpenGL_VertexArrayObject
    {
        uint id;
        Dictionary<uint, bool> attributeStatus;
        static OpenGL_VertexArrayObject Bound_VAO;

        public uint Pointer => id;

        private OpenGL_VertexArrayObject() { }

        public OpenGL_VertexArrayObject(uint name) {
            id = name;
            attributeStatus = new Dictionary<uint, bool>();
        }

        internal void EnableAttribute(uint attribute) {
            if (attributeStatus.TryGetValue(attribute,out bool enabled))
            {
                if (!enabled)
                {
                    Gl.EnableVertexAttribArray(attribute);
                    attributeStatus.Remove(attribute);
                    attributeStatus.Add(attribute, true);
                }
            } else
            {
                attributeStatus.Add(attribute, true);
                Gl.EnableVertexAttribArray(attribute);
            }
        }

        internal void DisableAttribute(uint attribute)
        {
            if (attributeStatus.TryGetValue(attribute, out bool enabled))
            {
                if (enabled)
                {
                    Gl.DisableVertexAttribArray(attribute);
                    attributeStatus.Remove(attribute);
                    attributeStatus.Add(attribute, false);
                }
            }
            else
            {
                attributeStatus.Add(attribute, false);
                Gl.DisableVertexAttribArray(attribute);
            }
        }

        internal void SetAttributePointer(uint location,int size, VertexAttribType attribType,bool normalized,int stride,int offset)
        {
            Gl.VertexAttribPointer(location, size, attribType, normalized, stride, (IntPtr)offset);
        }

        internal void Bind()
        {
            if (Bound_VAO == null || Bound_VAO.id != this.id)
            {
                Gl.BindVertexArray(id);
                OpenGL_Renderer.TestForGLErrors();
                Bound_VAO = this;
            }
        }

        internal void Unbind()
        {
            Gl.BindVertexArray(0);
            Bound_VAO = null;
        }

    }
}
