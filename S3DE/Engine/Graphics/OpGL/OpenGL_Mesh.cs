using OpenGL;
using S3DE;
using S3DE.Engine.Graphics;
using S3DE.Engine.Graphics.OpGL.BufferObjects;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Graphics.OpGL.OpenGL_Renderer;

namespace S3DE.Engine.Graphics.OpGL
{
    internal class OpenGL_Mesh : Renderer_Mesh
    {
        IOpenGL_BufferObject vbo, ebo;
        OpenGL_VertexArrayObject vao;

        int indicieCount;
        bool hasNormals, hasUvs;

        internal uint VAO_Pointer => vao.Pointer;
        internal int Indicies => indicieCount;

        internal OpenGL_Mesh()
        {
            vao = new OpenGL_VertexArrayObject(Gl.GenVertexArray());
            vao.Bind();
            vbo = OpenGL_BufferObject.CreateBuffer(BufferTarget.ArrayBuffer);
            vbo.Bind();
            ebo = OpenGL_BufferObject.CreateBuffer(BufferTarget.ElementArrayBuffer);
            ebo.Bind();

            identifier = vao.Pointer;
        }

        public override void Bind()
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

        public override void Unbind()
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

        float[] MeshDataToArray(System.Numerics.Vector3[] vertices, S3DE_Vector2[] uvs, System.Numerics.Vector3[] normals, System.Numerics.Vector4[] tangents, int[] indicies)
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
            System.Numerics.Vector3[] biTangents = null;
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
                    result.AddRange(tangents[i].XYZ().ToArray());
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



        internal void SetData(System.Numerics.Vector3[] vertices, S3DE_Vector2[] uvs, System.Numerics.Vector3[] normals, System.Numerics.Vector4[] tangents, int[] indicies, bool dynamic)
        {
            Bind();
            float[] vertData = MeshDataToArray(vertices, uvs, normals,tangents,indicies);
            ushort[] indicieData = IndiciesToUShortArray(indicies);

            //Console.WriteLine($"Uploading mesh {(vertData.Length * 4) + (indicieData.Length * 2)} bytes to GPU");
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

        public override void SetData(Mesh m) => SetData(m.Vertices, m.Uvs, m.Normals,m.Tangents, m.Indicies, m.IsDynamic);

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


        System.Numerics.Vector3[] CalculateBiTangents(System.Numerics.Vector3[] normals,System.Numerics.Vector4[] Tangents)
        {
            if (Tangents.Length != normals.Length)
                throw new ArgumentException("The mesh needs to have the same number of tangents as normals!");
            System.Numerics.Vector3[] BiTangents = new System.Numerics.Vector3[Tangents.Length];

            for (int i = 0; i < Tangents.Length; i++)
            {
                BiTangents[i] = System.Numerics.Vector3.Cross(normals[i], Tangents[i].XYZ()) * Tangents[i].W;
            }

            return BiTangents;
        }
    }
}
