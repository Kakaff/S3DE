using OpenGL;
using S3DE.Engine.Entities;
using S3DE.Engine.Graphics;
using SampleGame.Sample_OGL_Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_Components
{
    public sealed class Sample_CustomMeshRenderer : EntityComponent
    {
        public int TriangleCount
        {
            get => triangleCount;
            private set => triangleCount = value;
        }

        int triangleCount;
        OpenGL_BufferObject VBO,EBO;
        OpenGL_VertexArrayObject VAO;

        OpenGL_ShaderProgram shader;

        public void SetMesh(Mesh m)
        {
            //Convert mesh to OpenGL_Mesh and upload.
            if (m.Normals.Length != m.Vertices.Length && m.Uvs.Length != m.Vertices.Length)
                throw new ArgumentException();

            triangleCount = m.Indicies.Length / 3;

            float[] vertData = MeshVertDataToFloatArr(m);
            uint[] indicies = MeshIndiciesToUIntArr(m);


        }

        float[] MeshVertDataToFloatArr(Mesh m)
        {
            List<float> res = new List<float>();

            for (int i = 0; i < m.Vertices.Length; i++)
            {
                res.AddRange(m.Vertices[i].ToArray());
                res.AddRange(m.Uvs[i].ToArray());
                res.AddRange(m.Normals[i].ToArray());
            }

            return res.ToArray();
        }

        uint[] MeshIndiciesToUIntArr(Mesh m)
        {
            List<uint> res = new List<uint>();

            foreach (int i in m.Indicies)
                res.Add((uint)i);

            return res.ToArray();
        }

        protected override void Render()
        {
            shader.UseProgram();
            Bind();
            Gl.DrawElements(PrimitiveType.Triangles, TriangleCount * 3, DrawElementsType.UnsignedShort, IntPtr.Zero);
            //Set uniforms.
        }

        protected override void OnCreation()
        {
            VBO = new OpenGL_BufferObject(Gl.GenBuffer(), BufferTarget.ArrayBuffer);
            EBO = new OpenGL_BufferObject(Gl.GenBuffer(), BufferTarget.ElementArrayBuffer);
            VBO.Bind();
            EBO.Bind();
            VAO = new OpenGL_VertexArrayObject(Gl.GenVertexArray());
            VAO.Bind();

            SetVertArray();
        }

        void SetVertArray()
        {
            VAO.SetAttributePointer(0, 3, VertexAttribType.Float, false, 32, 0);  //Position
            VAO.SetAttributePointer(1, 2, VertexAttribType.Float, false, 32, 12); //Uvs
            VAO.SetAttributePointer(2, 3, VertexAttribType.Float, false, 32, 20); //Normals
            //Setup the vao.
        }

        void Bind()
        {
            VAO.Bind();
            VAO.EnableAttribute(0);
            VAO.EnableAttribute(1);
            VAO.EnableAttribute(2);
        }

        void Unbind()
        {
            VAO.DisableAttribute(0);
            VAO.DisableAttribute(1);
            VAO.DisableAttribute(2);
            VAO.Unbind();
        }
    }
}
