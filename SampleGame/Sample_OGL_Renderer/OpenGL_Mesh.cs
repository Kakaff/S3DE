using OpenGL;
using S3DE;
using S3DE.Engine.Graphics;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleGame.Sample_OGL_Renderer.OpenGL_Renderer;

namespace SampleGame.Sample_OGL_Renderer
{
    internal class OpenGL_Mesh
    {
        OpenGL_BufferObject vbo, ebo;
        OpenGL_VertexArrayObject vao;

        int indicieCount;

        internal int Indicies => indicieCount;

        internal OpenGL_Mesh()
        {
            vao = new OpenGL_VertexArrayObject(Gl.GenVertexArray());
            vao.Bind();
            vbo = new OpenGL_BufferObject(Gl.GenBuffer(),BufferTarget.ArrayBuffer);
            vbo.Bind();
            ebo = new OpenGL_BufferObject(Gl.GenBuffer(),BufferTarget.ElementArrayBuffer);
            ebo.Bind();
            vao.SetAttributePointer(0, 3, VertexAttribType.Float, false, 0, 0);
        }

        internal void Bind()
        {
            vao.Bind();
            vao.EnableAttribute(0);
        }
        internal void Unbind()
        {
            vao.DisableAttribute(0);
            vao.Unbind();
            vbo.Unbind();
            ebo.Unbind();
        }

        internal void SetData(Mesh m)
        {
            Bind();
            ValidateMesh(m);
            float[] vertData = Vector3.ToArray(m.Vertices);
            ushort[] indicies = IndiciesToUShortArray(m.Triangles);

            //Uploads the vertices to the GPU.
            using (MemoryLock ml = new MemoryLock(vertData))
            Gl.BufferData(BufferTarget.ArrayBuffer,
                (uint)(sizeof(float) * vertData.Length),
                ml.Address,
                (m.IsDynamic) ? BufferUsage.DynamicDraw : BufferUsage.StaticDraw);
            indicieCount = indicies.Length;
            TestForGLErrors();

            uint indicesSize = (uint)(2 * indicies.Length);

            using (MemoryLock ml = new MemoryLock(indicies))
                Gl.BufferData(BufferTarget.ElementArrayBuffer,
                    indicesSize,
                    ml.Address,
                    (m.IsDynamic) ? BufferUsage.DynamicDraw : BufferUsage.StaticDraw);
            TestForGLErrors();
            Unbind();
        }

        ushort[] IndiciesToUShortArray(int[] arr)
        {
            List<ushort> res = new List<ushort>();

            for (int i = 0; i < arr.Length; i++)
                if (arr[i] <= ushort.MaxValue)
                    res.Add((ushort)arr[i]);
                else
                    throw new ArgumentOutOfRangeException($"A mesh may not have more than {ushort.MaxValue} vertices!");

            return res.ToArray();
        }

        void ValidateMesh(Mesh m)
        {
            //Should probably implement this at some point.
        }


    }
}
