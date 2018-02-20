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
            vbo = new OpenGL_BufferObject(Gl.GenBuffer(), BufferTarget.ArrayBuffer);
            vbo.Bind();
            ebo = new OpenGL_BufferObject(Gl.GenBuffer(), BufferTarget.ElementArrayBuffer);
            ebo.Bind();
        }

        internal void Bind()
        {
            vao.Bind();
            vao.EnableAttribute(0);
            if (hasUvs)
                vao.EnableAttribute(1);
            if (hasNormals)
            {
                vao.EnableAttribute(2);
                vao.EnableAttribute(3);
                vao.EnableAttribute(4);
            }
        }

        internal void Unbind()
        {
            vao.DisableAttribute(0);
            if (hasUvs)
                vao.DisableAttribute(1);
            if (hasNormals)
            {
                vao.DisableAttribute(2);
                vao.DisableAttribute(3);
                vao.DisableAttribute(4);
            }
            vao.Unbind();
            vbo.Unbind();
            ebo.Unbind();
        }

        float[] MeshDataToArray(Vector3[] vertices, Vector2[] uvs, Vector3[] normals, Vector4[] tangents, int[] indicies)
        {
            if (uvs.Length == vertices.Length)
                hasUvs = true;
            else if (uvs.Length > 0)
                throw new ArgumentException("Uvs count need to be the same as vertice count or 0");
            else
                hasUvs = false;

            if (normals.Length == vertices.Length && hasUvs)
                hasNormals = true;
            else if (normals.Length > 0)
                throw new ArgumentException("Normals count need to be the same as uv count or 0");
            else
                hasNormals = false;

            //If has normals, calc tangents and bitangents.
            Vector3[] biTangents = null;
            if (hasNormals)
                biTangents = CalculateBiTangents(normals, tangents);


            List<float> result = new List<float>();

            for (int i = 0; i < vertices.Length; i++)
            {
                result.AddRange(vertices[i].ToArray());
                if (hasUvs)
                    result.AddRange(uvs[i].ToArray());
                if (hasNormals)
                {
                    result.AddRange(normals[i].ToArray());
                    result.AddRange(tangents[i].xyz.ToArray());
                    result.AddRange(biTangents[i].ToArray());
                }
            }

            return result.ToArray();
        }

        internal void SetVao(bool uv, bool normals)
        {
            if (uv && normals)
            {
                vao.Bind();
                vao.SetAttributePointer(0, 3, VertexAttribType.Float, false, 56, 0);  //pos
                vao.SetAttributePointer(1, 2, VertexAttribType.Float, false, 56, 12); //uv
                vao.SetAttributePointer(2, 3, VertexAttribType.Float, false, 56, 20); //normal
                vao.SetAttributePointer(3, 3, VertexAttribType.Float, false, 56, 32); //Tangent
                vao.SetAttributePointer(4, 3, VertexAttribType.Float, false, 56, 44); //BiTangent

            } else if (uv)
            {
                vao.SetAttributePointer(0, 3, VertexAttribType.Float, false, 20, 0);
                vao.SetAttributePointer(1, 2, VertexAttribType.Float, false, 20, 12);
            }
        }



        internal void SetData(Vector3[] vertices, Vector2[] uvs, Vector3[] normals, Vector4[] tangents, int[] indicies, bool dynamic)
        {
            Bind();
            float[] vertData = MeshDataToArray(vertices, uvs, normals,tangents,indicies);
            ushort[] indicieData = IndiciesToUShortArray(indicies);

            Console.WriteLine($"Uploading mesh {(vertData.Length * 4) + (indicieData.Length * 2)} bytes to GPU");
            //Uploads the vertices,uvs and normals to the GPU.
            using (MemoryLock ml = new MemoryLock(vertData))
                Gl.BufferData(BufferTarget.ArrayBuffer,
                    (uint)(sizeof(float) * vertData.Length),
                    ml.Address,
                    (dynamic) ? BufferUsage.DynamicDraw : BufferUsage.StaticDraw);
            indicieCount = indicies.Length;
            TestForGLErrors();

            SetVao(hasUvs, hasNormals);

            uint indicesSize = (uint)(2 * indicies.Length);

            using (MemoryLock ml = new MemoryLock(indicieData))
                Gl.BufferData(BufferTarget.ElementArrayBuffer,
                    indicesSize,
                    ml.Address,
                    (dynamic) ? BufferUsage.DynamicDraw : BufferUsage.StaticDraw);
            TestForGLErrors();
            Unbind();
        }

        internal void SetData(Mesh m) => SetData(m.Vertices, m.Uvs, m.Normals,m.Tangents, m.Indicies, m.IsDynamic);

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


        Vector3[] CalculateBiTangents(Vector3[] normals,Vector4[] Tangents)
        {
            if (Tangents.Length != normals.Length)
                throw new ArgumentException("The mesh needs to have the same number of tangents as normals!");
            Vector3[] BiTangents = new Vector3[Tangents.Length];

            for (int i = 0; i < Tangents.Length; i++)
            {
                BiTangents[i] = Vector3.Cross(normals[i], Tangents[i].xyz) * Tangents[i].w;
            }

            return BiTangents;
        }
    }
}
