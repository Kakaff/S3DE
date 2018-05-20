using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public abstract class Renderer_Mesh
    {
        public Renderer_Mesh Create() => Renderer.CreateMesh();

        protected uint identifier;
        public uint Identifier => identifier;

        public abstract void Bind();
        public abstract void Unbind();
        public abstract void SetData(Mesh m);
    }
}
