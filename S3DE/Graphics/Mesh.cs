using S3DE.Maths;
using S3DE.Utility;
using System;

namespace S3DE.Graphics
{
    public partial class Mesh
    {
        static Mesh activeMesh;

        IntPtr handle;

        Vector3[] verts;
        Vector2[] uvs;
        ushort[] indicies;

        public Vector3[] Vertices {set => verts = value;}
        public Vector2[] UVs { set => uvs = value; }
        public ushort[] Indicies { set => indicies = value; }

        bool isEmpty,isBound;

        public Mesh()
        {
            handle = CreateMesh();
        }

        public static Mesh CreateCube(Vector3 scale)
        {
            Mesh m = new Mesh();

            m.SetVertexAttribute(0, 3, GLType.FLOAT, false, 20, 0);
            m.SetVertexAttribute(1, 2, GLType.FLOAT, false, 20, 12);
            m.EnableVertexAttribute(0);
            m.EnableVertexAttribute(1);

            float vX = 0.5f * scale.x;
            float vY = 0.5f * scale.y;
            float vZ = 0.5f * scale.z;

            m.Vertices = new Vector3[] {new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f),new Vector3(0.5f,-0.5f,-0.5f),
                                        new Vector3(-0.5f,0.5f,-0.5f),new Vector3(-0.5f,0.5f,0.5f),new Vector3(0.5f,0.5f,0.5f),new Vector3(0.5f,0.5f,-0.5f),
                                        new Vector3(0.5f,-0.5f,-0.5f),new Vector3(0.5f,0.5f,-0.5f),new Vector3(0.5f,0.5f,0.5f),new Vector3(0.5f,-0.5f,0.5f),
                                        new Vector3(-0.5f,-0.5f,0.5f),new Vector3(-0.5f,0.5f,0.5f),new Vector3(-0.5f,0.5f,-0.5f),new Vector3(-0.5f,-0.5f,-0.5f),
                                        new Vector3(0.5f,-0.5f,0.5f),new Vector3(0.5f,0.5f,0.5f),new Vector3(-0.5f,0.5f,0.5f),new Vector3(-0.5f,-0.5f,0.5f),
                                        new Vector3(-0.5f,-0.5f,0.5f),new Vector3(-0.5f,-0.5f,-0.5f),new Vector3(0.5f,-0.5f,-0.5f),new Vector3(0.5f,-0.5f,0.5f)};

            m.UVs = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                                    new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                                    new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                                    new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                                    new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                                    new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)};


            m.Indicies = new ushort[] { 0,1,2,2,3,0,
                                        4,5,6,6,7,4,
                                        8,9,10,10,11,8,
                                        12,13,14,14,15,12,
                                        16,17,18,18,19,16,
                                        20,21,22,22,23,20};
            m.Apply();

            return m;
        }

        public void SetVertexAttribute(uint index, int size, GLType type, bool normalized, uint stride, uint offset)
        {
            if (!isBound)
                Bind();

            SetVertexAttrib(handle, index, size, type, normalized, stride, offset);
            if (!Renderer.NoError)
                throw new Exception("Error setting vertex attribute!");
        }

        public void EnableVertexAttribute(uint index)
        {
            if (!isBound)
                Bind();

            EnableVertexAttrib(handle, index);
            if (!Renderer.NoError)
                throw new Exception("Error enabling vertex attribute!");
        }

        void Bind()
        {
            if (!isBound)
            {
                if (activeMesh != null)
                    activeMesh.isBound = false;

                Extern_Mesh_Bind(handle);
                activeMesh = this;
                isBound = true;
            }
        }

        public void Draw()
        {
            if (!isBound)
                 Bind();

            DrawMesh(handle);
        }

        public void Apply()
        {
            if (!isBound)
                Bind();

            using (ByteBuffer V_BB = ByteBuffer.Create(verts.Length * 4))
            {
                using (ByteBuffer I_BB = ByteBuffer.Create(indicies.Length * 2)) {

                    for (int i = 0; i < verts.Length; i++)
                    {
                        Vector3 v = verts[i];
                        Vector2 uv = uvs[i];
                        V_BB.AddRange(4, v.x, v.y, v.z);
                        V_BB.AddRange(4, uv.x, uv.y);
                    }
                    I_BB.AddRange(2, indicies);
                    using (PinnedMemory vpm = new PinnedMemory(V_BB.Data)) {
                        using (PinnedMemory ipm = new PinnedMemory(I_BB.Data))
                            SetMeshData(handle, V_BB.Data, (uint)V_BB.Length, 
                                I_BB.Data, (uint)I_BB.Length,
                                BufferUsage.STATIC_DRAW);
                    }


                    isEmpty = I_BB.Data.Length == 0 && V_BB.Data.Length == 0;
                }
            }
        }
    }
}
