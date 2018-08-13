using S3DE.Engine.Collections;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public abstract partial class Renderer
    {
        public static UniformBuffer CreateUniformBuffer()
        {
            UniformBuffer ub = new UniformBuffer(ActiveRenderer.Create_UniformBuffer());
            ub.SetSize(32);
            return ub;
        }

        public static void BindUniformBuffer(UniformBuffer buffer, int bindingPoint) => ActiveRenderer.Bind_UniformBuffer(buffer, bindingPoint);
        public static void UnbindUniformBuffer(UniformBuffer buffer) => ActiveRenderer.Unbind_UniformBuffer(buffer);
        public static void SetUniformBufferData(UniformBuffer buffer, byte[] data) => ActiveRenderer.Set_UniformBufferData(buffer, data);
        public static void SetUniformBufferData(UniformBuffer buffer, int offset, byte[] data) => ActiveRenderer.Set_UniformBufferData(buffer, offset, data);
        public static void SetUniformBufferSize(UniformBuffer buffer, uint size) => ActiveRenderer.Set_UniformBufferSize(buffer,size);

        internal static Framebuffer CreateFramebuffer_Internal(S3DE_Vector2 size) => ActiveRenderer.CreateFrameBuffer((int)size.X, (int)size.Y);
        internal static void SetDrawBuffers_Internal(params BufferAttachment[] buffers) => ActiveRenderer.SetDrawBuffers(buffers);

        protected void UpdateBufferSize(UniformBuffer buffer, uint size) => buffer.SetSize(size);
        protected abstract uint Create_UniformBuffer();
        protected abstract void SetDrawBuffers(BufferAttachment[] buffers);
        protected abstract void Set_UniformBufferData(UniformBuffer buffer,byte[] data);
        protected abstract void Set_UniformBufferData(UniformBuffer buffer, int offset, byte[] data);
        protected abstract void Set_UniformBufferSize(UniformBuffer buffer, uint size);
        protected abstract void Bind_UniformBuffer(UniformBuffer buffer, int bindingPoint);
        protected abstract void Unbind_UniformBuffer(UniformBuffer buffer);
    }
}
