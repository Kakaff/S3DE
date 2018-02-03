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
        bool hasNormals, hasUvs;

        internal int Indicies => indicieCount;

        internal OpenGL_Mesh()
        {
            vao = new OpenGL_VertexArrayObject(Gl.GenVertexArray());
            vao.Bind();
            vbo = new OpenGL_BufferObject(Gl.GenBuffer(),BufferTarget.ArrayBuffer);
            vbo.Bind();
            ebo = new OpenGL_BufferObject(Gl.GenBuffer(),BufferTarget.ElementArrayBuffer);
            ebo.Bind();
            //Should probably make it so that the vao can change the attributes depending on if there are any uvs/normals or not.
            //add Vertices.
            vao.SetAttributePointer(0, 3, VertexAttribType.Float, false, 32, 0);
            //Add Uvs
            vao.SetAttributePointer(1, 2, VertexAttribType.Float, false, 32, 12);
            //add normals
            vao.SetAttributePointer(2, 3, VertexAttribType.Float, false, 32, 20);
        }

        internal void Bind()
        {
            vao.Bind();
            vao.EnableAttribute(0);
            vao.EnableAttribute(1);
            vao.EnableAttribute(2);
        }

        internal void Unbind()
        {
            vao.DisableAttribute(0);
            vao.DisableAttribute(1);
            vao.DisableAttribute(2);
            vao.Unbind();
            vbo.Unbind();
            ebo.Unbind();
        }

        float[] MeshDataToArray(Vector3[] vertices,Vector2[] uvs, Vector3[] normals)
        {
            if (uvs.Length != vertices.Length && normals.Length != vertices.Length)
            {
                throw new ArgumentException($"Mesh needs to have the same number of uvs and normals as vertices! " +
                    $"| Vertices: {vertices.Length} | Uvs: {uvs.Length} | Normals: {normals.Length}");
            }

            List<float> result = new List<float>();

            for (int i = 0; i < vertices.Length; i++)
            {
                result.AddRange(vertices[i].ToArray());
                result.AddRange(uvs[i].ToArray());
                result.AddRange(normals[i].ToArray());
            }

            return result.ToArray();
        }

        internal void SetData(Mesh m)
        {
            Bind();
            float[] vertData = MeshDataToArray(m.Vertices,m.Uvs,m.Normals);
            ushort[] indicies = IndiciesToUShortArray(m.Triangles);

            //Uploads the vertices,uvs and normals to the GPU.
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
    }
}
