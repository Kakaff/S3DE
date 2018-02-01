using OpenGL;
using Khronos;

using S3DE.Engine.Entities;
using S3DE.Engine.Graphics;
using S3DE.Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE;

namespace SampleGame.Sample_OGL_Renderer
{
    public sealed class OpenGL_Renderer : Renderer
    {
        protected override void clear()
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit | 
                     ClearBufferMask.DepthBufferBit);
        }

        protected override Renderer_Material createMaterial(Type materialType)
        {
            return new OpenGL_Material();
        }

        protected override Renderer_MeshRenderer createMeshRenderer()
        {
            return new OpenGL_MeshRenderer();
        }

        protected override void init()
        {
            Gl.Initialize();
        }

        protected override void SetCapabilities()
        {
            Gl.FrontFace(FrontFaceDirection.Cw);
            TestForGLErrors();
            Gl.Enable(EnableCap.DepthTest);
            TestForGLErrors();
            Gl.CullFace(CullFaceMode.Back);
            TestForGLErrors();
            Gl.FrontFace(FrontFaceDirection.Cw);
            TestForGLErrors();
            Gl.Enable(EnableCap.CullFace);
            TestForGLErrors();
        }

        internal static void TestForGLErrors()
        {
            ErrorCode err = Gl.GetError();
            if (err != ErrorCode.NoError)
            {
                throw new Exception("GL Operation Failed, Error: " + err);
            }
        }

        protected override void OnWindowResized()
        {
            Gl.Viewport(0, 0, (int)Game.Resolution.x, (int)Game.Resolution.y);
        }
    }
}
