using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using S3DE.Engine.Data;
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

        public override void SetData(byte[] data)
        {

            Gl.BindBuffer(BufferTarget.UniformBuffer, identifier);
            OpenGL_Renderer.TestForGLErrors();
            using (MemoryLock ml = new MemoryLock(data))
                Gl.BufferData(BufferTarget.UniformBuffer, (uint)data.Length, ml.Address, BufferUsage.DynamicDraw);

            OpenGL_Renderer.TestForGLErrors();
        }
    }
}
