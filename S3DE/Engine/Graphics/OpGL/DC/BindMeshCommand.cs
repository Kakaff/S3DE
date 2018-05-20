using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.DC
{
    class BindMeshCommand : IDrawCallCommand
    {
        private BindMeshCommand() { }

        Renderer_Mesh mesh;

        internal BindMeshCommand(Renderer_Mesh m)
        {
            mesh = m;
        }

        public void OnAdd(DrawCall dc) => dc.SetMesh(mesh);
        public void Dispose() => mesh = null;
        public void Perform() => mesh.Bind();
    }
}
