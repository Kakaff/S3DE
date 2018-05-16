using OpenGL;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Graphics.OpGL.OpenGL_Renderer;

namespace S3DE.Engine.Graphics.OpGL.DC
{
    interface IDrawCallCommand
    {
        void Perform();
        void Dispose();
        void OnAdd(DrawCall dc);
    }
}
