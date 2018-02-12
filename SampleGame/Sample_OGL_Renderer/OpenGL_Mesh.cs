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

        float[] MeshDataToArray(Vector3[] vertices, Vector2[] uvs, Vector3[] normals, int[] indicies)
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
            Vector3[] tangents = null,bitangents = null;
            if (hasNormals)
            {
                (Vector3[] Tangents, Vector3[] BiTangents) t = CalculateTangents(vertices, uvs, normals, indicies);
                tangents = t.Tangents;
                bitangents = t.BiTangents;
            }

            List<float> result = new List<float>();

            for (int i = 0; i < vertices.Length; i++)
            {
                result.AddRange(vertices[i].ToArray());
                if (hasUvs)
                    result.AddRange(uvs[i].ToArray());
                if (hasNormals)
                {
                    result.AddRange(normals[i].ToArray());
                    result.AddRange(tangents[i].ToArray());
                    result.AddRange(bitangents[i].ToArray());
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

        

        internal void SetData(Vector3[] vertices, Vector2[] uvs, Vector3[] normals, int[] indicies, bool dynamic)
        {
            Bind();
            float[] vertData = MeshDataToArray(vertices, uvs, normals,indicies);
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

        internal void SetData(Mesh m) => SetData(m.Vertices, m.Uvs, m.Normals, m.Triangles, m.IsDynamic);

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


        (Vector3[] Tangents, Vector3[] BiTangents) CalculateTangents(Vector3[] vertices, Vector2[] uvs, Vector3[] normals, int[] indicies)
        {
            Console.WriteLine("Calculating tangents");
            //Should probably move this into the S3DE.Mesh so it is calculated along with the normals.
            Vector3[] Tangents = new Vector3[vertices.Length];
            Vector3[] BiTangents = new Vector3[vertices.Length];

            for (int i = 0; i < indicies.Length; i+= 3)
            {
                
                int i0 = indicies[i];
                int i1 = indicies[i + 1];
                int i2 = indicies[i + 2];
                
                Vector3 v0 = vertices[i0];
                Vector3 v1 = vertices[i1];
                Vector3 v2 = vertices[i2];

                Vector2 uv0 = uvs[i0];
                Vector2 uv1 = uvs[i1];
                Vector2 uv2 = uvs[i2];

                Vector3 edge1 = v1 - v0;
                Vector3 edge2 = v2 - v0;

                Vector2 deltaUV0 = uv1 - uv0;
                Vector2 deltaUV1 = uv2 - uv0;

                float r = 1.0f / (deltaUV0.x * deltaUV1.y - deltaUV1.x * deltaUV0.y);

                Vector3 tang = (edge1 * deltaUV1.y - edge2 * deltaUV0.y) * r;
                Vector3 bitang = (edge1 * -deltaUV1.x + edge2 * deltaUV0.x) * r;

                Tangents[i0] += tang;
                Tangents[i1] += tang;
                Tangents[i2] += tang;

                BiTangents[i0] += bitang;
                BiTangents[i1] += bitang;
                BiTangents[i2] += bitang;
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                Tangents[i] = Tangents[i].normalized;
                BiTangents[i] = BiTangents[i].normalized;
            }

            //Normalize all tangents and bitangents.
            return (Tangents,BiTangents);
        }
    }
}
