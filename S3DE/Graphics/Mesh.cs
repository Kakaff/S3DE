using S3DE.Maths;
using S3DE.Utility;
using System;

namespace S3DE.Graphics
{
    public partial class Mesh
    {
        IntPtr handle;

        Vector3[] verts;
        Vector2[] uvs;
        ushort[] indicies;

        public Vector3[] Vertices {set => verts = value;}
        public Vector2[] UVs { set => uvs = value; }
        public ushort[] Indicies { set => indicies = value; }

        public Mesh()
        {
            handle = CreateMesh();
        }

        public void SetVertexAttribute(uint index, int size, GLType type, bool normalized, uint stride, uint offset)
        {
            SetVertexAttrib(handle, index, size, type, normalized, stride, offset);
        }

        public void EnableVertexAttribute(uint index)
        {
            EnableVertexAttrib(handle, index);
        }

        public void Draw()
        {
            DrawMesh(handle);
        }

        public void Apply()
        {
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
                }
            }
        }
    }
}
