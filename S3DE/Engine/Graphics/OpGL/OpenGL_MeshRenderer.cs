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

namespace S3DE.Engine.Graphics.OpGL
{
    internal sealed class OpenGL_MeshRenderer : Renderer_MeshRenderer
    {
        OpenGL_Mesh mesh;
        Mesh m;

        internal OpenGL_Mesh InternalMesh => mesh;

        protected override void PrepareRender()
        {

        }

        protected override void Render()
        {
            if (mesh != null)
            {
                mesh.Bind();
                Renderer.DrawMesh(mesh);
                OpenGL_Renderer.TestForGLErrors();
            }
        }

        protected override void SetMesh(Mesh m)
        {
            this.m = m;
            mesh = m.InternalMesh as OpenGL_Mesh;
        }
    }
}
