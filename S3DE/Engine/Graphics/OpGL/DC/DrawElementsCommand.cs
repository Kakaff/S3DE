using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.DC
{
    class DrawElementsCommand : IDrawCallCommand
    {
        
        Renderer_Mesh mesh;

        internal DrawElementsCommand()
        {
        
        }

        public void OnAdd(DrawCall dc) {mesh = dc.Mesh;}

        public void Dispose() => mesh = null;
        public void Perform()
        {
            Renderer.DrawMesh(mesh);
        }
    }
}
