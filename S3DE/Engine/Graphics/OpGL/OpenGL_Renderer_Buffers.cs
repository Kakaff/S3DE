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
            using (MemoryLock ml = new MemoryLock(data))
                if (API_Version >= 450)
                {
                    if (buffer.Size < data.Length)
                    {
                        Gl.NamedBufferData(buffer.Identifier, (uint)data.Length, ml.Address, BufferUsage.DynamicDraw);
                        UpdateBufferSize(buffer, (uint)data.Length);
                    }
                    else
                        Gl.NamedBufferSubData(buffer.Identifier, IntPtr.Zero, (uint)data.Length, ml.Address);

                } else
                {
                    if (buffer.IsBound)
                        UniformBuffers.Unbind(buffer.BoundUniformBlockBindingPoint);

                    Gl.BindBuffer(BufferTarget.UniformBuffer, buffer.Identifier);
                    TestForGLErrors();
                    if (buffer.Size < data.Length)
                    {
                        Gl.BufferData(BufferTarget.UniformBuffer, (uint)data.Length, ml.Address, BufferUsage.DynamicDraw);
                        UpdateBufferSize(buffer, (uint)data.Length);
                    }
                    else
                        Gl.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, (uint)data.Length, ml.Address);
                }
            TestForGLErrors();
        }

        protected override void Set_UniformBufferData(UniformBuffer buffer, int offset, byte[] data)
        {
            using (MemoryLock ml = new MemoryLock(data))
                if (buffer.Size < offset + data.Length)
                    throw new ArgumentOutOfRangeException("The buffer is too small to fit the data");
                else
                    Gl.NamedBufferSubData(buffer.Identifier, new IntPtr(offset), (uint)data.Length, ml.Address);

            TestForGLErrors();
        }

        protected override void Set_UniformBufferSize(UniformBuffer buffer, uint size)
        {
            Gl.NamedBufferData(buffer.Identifier, size, IntPtr.Zero, BufferUsage.DynamicDraw);
            TestForGLErrors();
            UpdateBufferSize(buffer, size);
        }

        protected override uint Create_UniformBuffer()
        {
            uint i = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.UniformBuffer, i);
            Gl.BufferData(BufferTarget.UniformBuffer, 32, IntPtr.Zero, BufferUsage.DynamicDraw);
            TestForGLErrors();
            return i;
        }

    }
}
