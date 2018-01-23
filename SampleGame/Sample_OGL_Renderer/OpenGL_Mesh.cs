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

        int triangleCount;

        internal int Indicies => triangleCount;

        internal OpenGL_Mesh()
        {
            vbo = new OpenGL_BufferObject(Gl.GenBuffer(),BufferTarget.ArrayBuffer);
            //ebo = new OpenGL_BufferObject(Gl.GenBuffer(),BufferTarget.ElementArrayBuffer);
            vao = new OpenGL_VertexArrayObject(Gl.GenVertexArray());
        }

        internal void Bind()
        {
            //ebo.Bind();
            vao.Bind();
            vbo.Bind();

            vao.EnableAttribute(0);
        }

        internal void SetData(Mesh m)
        {
            Bind();
            ValidateMesh(m);
            float[] vertData = VerticeDataToFloatArray(m);
            ushort[] indicies = IndiciesToUShortArray(m.Triangles);
            
            //Uploads the vertices to the GPU.
            using (MemoryLock ml = new MemoryLock(vertData))
            Gl.BufferData(BufferTarget.ArrayBuffer,
                (uint)(sizeof(float) * vertData.Length),
                ml.Address,
                (m.IsDynamic) ? BufferUsage.DynamicDraw : BufferUsage.StaticDraw);
            triangleCount = m.Triangles.Length;

            TestForGLErrors();

            /*
            //Crashes for some odd reason 
            //Uploads the indicies to the GPU
            using (MemoryLock ml = new MemoryLock(indicies))
                Gl.BufferData(BufferTarget.ElementArrayBuffer,
                    (uint)(sizeof(ushort) * indicies.Length),
                    ml.Address,
                    (m.IsDynamic) ? BufferUsage.DynamicDraw : BufferUsage.StaticDraw);
            //TestForGLErrors();
            */

            //Sets up the VAO. For now we only set the positions
            //In the future, add uvs/normals.
            vao.SetAttributePointer(0, 3, Gl.FLOAT, false, 0, 0);
            TestForGLErrors();
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
        float[] VerticeDataToFloatArray(Mesh m)
        {
            List<float> vertData = new List<float>();
            //Add uv and normals later.
            for (int i = 0; i < m.Vertices.Length; i++)
            {
                vertData.AddRange(m.Vertices[i].ToArray());
            }
            
            return vertData.ToArray();
        }

        void ValidateMesh(Mesh m)
        {
            //If the number of normals or uvs is the same as the vertices or 0 then it's valid.
            //Check to see if all indicies are refering to valid vertices aswell.
        }


    }
}
