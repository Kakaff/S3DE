using S3DE.Engine.Graphics.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.DC
{
    class UseRendererMaterialCommand : IDrawCallCommand
    {
        private UseRendererMaterialCommand() { }

        Renderer_Material rMat;

        internal UseRendererMaterialCommand(Renderer_Material mat)
        {
            rMat = mat;
        }

        public void OnAdd(DrawCall dc) => dc.SetRendererMaterial(rMat);
        public void Dispose() => rMat = null;
        public void Perform() => rMat.UseRendererMaterial();
    }
}
