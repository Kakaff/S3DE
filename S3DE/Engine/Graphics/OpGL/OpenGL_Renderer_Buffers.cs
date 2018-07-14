using OpenGL;
using S3DE.Engine.Collections;
using S3DE.Engine.Graphics.OpGL.BufferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL
{
    public sealed partial class OpenGL_Renderer : Renderer
    {
        protected override void Bind_UniformBuffer(UniformBuffer buffer, int bindingPoint)
        {
            Gl.BindBufferBase(BufferTarget.UniformBuffer, (uint)bindingPoint, buffer.Identifier);
            TestForGLErrors();
        }

        protected override void Unbind_UniformBuffer(UniformBuffer buffer)
        {
            Gl.BindBufferBase(BufferTarget.UniformBuffer, (uint)0, 0);
            TestForGLErrors();
        }

        protected override void Set_UniformBufferData(UniformBuffer buffer, byte[] data)
        {
            if (buffer.IsBound)
                UniformBuffers.Unbind(buffer.BoundUniformBlockBindingPoint);

            Gl.BindBuffer(BufferTarget.UniformBuffer, buffer.Identifier);
            TestForGLErrors();
            using (MemoryLock ml = new MemoryLock(data))
                Gl.BufferData(BufferTarget.UniformBuffer, (uint)data.Length, ml.Address, BufferUsage.DynamicDraw);
            TestForGLErrors();
        }

        protected override uint Create_UniformBuffer()
        {
            uint i = Gl.GenBuffer();
            TestForGLErrors();
            return i;
        }

    }
}
