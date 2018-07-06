using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using S3DE.Engine.Collections;
using S3DE.Engine.Graphics.Materials;

namespace S3DE.Engine.Graphics.OpGL.BufferObjects
{
    internal class OpenGL_UniformBufferObject : S3DE_UniformBuffer,IOpenGL_BufferObject
    {
        uint identifier;

        private OpenGL_UniformBufferObject() { }

        internal OpenGL_UniformBufferObject(uint identifier) => this.identifier = identifier;

        public override uint Identifier => identifier;

        public BufferTarget BufferTarget => BufferTarget.UniformBuffer;
        

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Unbind()
        {
            throw new NotImplementedException();
        }

        protected override void Resize(uint size)
        {
            Gl.BindBuffer(BufferTarget.UniformBuffer, identifier);
            OpenGL_Renderer.TestForGLErrors();
            Gl.BufferData(BufferTarget.UniformBuffer, size, IntPtr.Zero, BufferUsage.DynamicDraw);
            OpenGL_Renderer.TestForGLErrors();
        }

        public override void SetData(byte[] data)
        {
            if (Size >= data.Length)
            {
                Gl.BindBuffer(BufferTarget.UniformBuffer, identifier);
                OpenGL_Renderer.TestForGLErrors();
                using (MemoryLock ml = new MemoryLock(data))
                {
                    //Gl.BufferData is faster on Intel GPU but slower on Nvidia/AMD... Stupid Intel.
                    //Gl.BufferData(BufferTarget.UniformBuffer, (uint)data.Length, ml.Address, BufferUsage.DynamicDraw);
                    Gl.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, (uint)data.Length, ml.Address);
                }
                OpenGL_Renderer.TestForGLErrors();
            }
            else
                throw new ArgumentOutOfRangeException($"Trying to set {data.Length} bytes but the UniformBuffer is only {Size} bytes");
        }
    }
}
