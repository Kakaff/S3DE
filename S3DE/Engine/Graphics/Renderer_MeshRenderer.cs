using S3DE.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public abstract class Renderer_MeshRenderer
    {
        internal void Render_Internal() => Render();
        internal void Render_DC_Internal() => Render_DC();

        internal void Prepare_Internal() => PrepareRender();

        internal void SetMesh_Internal(Mesh m) => SetMesh(m);
        
        protected abstract void PrepareRender();
        protected abstract void Render();
        protected abstract void Render_DC();
        protected abstract void SetMesh(Mesh m);
        
    }
}
