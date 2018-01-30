using OpenGL;
using Khronos;
using S3DE.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Maths;
using S3DE.Engine.Graphics;

namespace SampleGame.Sample_OGL_Renderer
{
    internal sealed class OpenGL_MeshRenderer : Renderer_MeshRenderer
    {
        OpenGL_Mesh mesh;
        Mesh m;

        bool hasChanged = false;

        protected override void PrepareRender()
        {
            if (hasChanged)
            {
                //Validate the mesh.
                if (mesh == null)
                    mesh = new OpenGL_Mesh();

                mesh.SetData(m);
                hasChanged = false;
            }
        }

        protected override void Render()
        {
            if (mesh != null)
            {
                mesh.Bind();
                Gl.DrawElements(PrimitiveType.Triangles, mesh.Indicies, DrawElementsType.UnsignedShort, IntPtr.Zero);
                OpenGL_Renderer.TestForGLErrors();
                mesh.Unbind();
            }
        }

        protected override void SetMesh(Mesh m)
        {
            this.m = m;
            hasChanged = true;
        }
    }
}
