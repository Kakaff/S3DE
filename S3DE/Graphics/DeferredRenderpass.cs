using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics
{
    public sealed class DeferredRenderpass : Renderpass
    {
        protected override void Init()
        {
            
        }

        protected override void OnDraw()
        {
            DrawMeshes();
        }

        protected override void PostDraw()
        {
            
        }

        protected override void PreDraw()
        {
            
        }
    }
}
