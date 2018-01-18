using OpenGL;
using S3DE.Engine.Entities;
using S3DE.Engine.Graphics;
using S3DE.Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_OGL_Renderer
{
    public sealed class OpenGL_Renderer : Renderer
    {
        protected override void clear()
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit | 
                     ClearBufferMask.DepthBufferBit |
                     ClearBufferMask.StencilBufferBit);
        }

        protected override Renderer_Material createMaterial(Type materialType)
        {
            throw new NotImplementedException();
        }

        protected override Renderer_MeshRenderer createMeshRenderer()
        {
            throw new NotImplementedException();
        }

        protected override void init()
        {
            Gl.Initialize();

            Gl.Enable(EnableCap.DepthTest);
            Gl.Enable(EnableCap.CullFace);
            Gl.CullFace(CullFaceMode.Back);
            Gl.FrontFace(FrontFaceDirection.Cw);
        }
    }
}
