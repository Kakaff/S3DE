using S3DE.Maths;
using S3DE.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Meshes
{
    public class StandardMesh : Mesh
    {
        public Vector3[] Vertices { private get; set; }
        public Vector2[] Uvs { private get; set; }
        public ushort[] Indicies { private get; set; }

        public StandardMesh() : base()
        {
            SetVertexAttribute(new VertexAttribute(0, 3, GLType.FLOAT, false, 20, 0));
            SetVertexAttribute(new VertexAttribute(1, 2, GLType.FLOAT, false, 20, 12));
            EnableVertexAttribute(0);
            EnableVertexAttribute(1);
        }

        public static StandardMesh CreateCube(Vector3 scale)
        {
            StandardMesh m = new StandardMesh();

            float vX = 0.5f * scale.x;
            float vY = 0.5f * scale.y;
            float vZ = 0.5f * scale.z;

            m.Vertices = new Vector3[] {new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f),new Vector3(0.5f,-0.5f,-0.5f),
                                        new Vector3(-0.5f,0.5f,-0.5f),new Vector3(-0.5f,0.5f,0.5f),new Vector3(0.5f,0.5f,0.5f),new Vector3(0.5f,0.5f,-0.5f),
                                        new Vector3(0.5f,-0.5f,-0.5f),new Vector3(0.5f,0.5f,-0.5f),new Vector3(0.5f,0.5f,0.5f),new Vector3(0.5f,-0.5f,0.5f),
                                        new Vector3(-0.5f,-0.5f,0.5f),new Vector3(-0.5f,0.5f,0.5f),new Vector3(-0.5f,0.5f,-0.5f),new Vector3(-0.5f,-0.5f,-0.5f),
                                        new Vector3(0.5f,-0.5f,0.5f),new Vector3(0.5f,0.5f,0.5f),new Vector3(-0.5f,0.5f,0.5f),new Vector3(-0.5f,-0.5f,0.5f),
                                        new Vector3(-0.5f,-0.5f,0.5f),new Vector3(-0.5f,-0.5f,-0.5f),new Vector3(0.5f,-0.5f,-0.5f),new Vector3(0.5f,-0.5f,0.5f)};

            m.Uvs = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
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

        public override void Apply()
        {
            using (ByteBuffer V_BB = ByteBuffer.Create(Vertices.Length * 4))
            {

                for (int i = 0; i < Vertices.Length; i++)
                {
                    Vector3 v = Vertices[i];
                    Vector2 uv = Uvs[i];
                    V_BB.AddRange(4, v.x, v.y, v.z);
                    V_BB.AddRange(4, uv.x, uv.y);
                }

                UploadMeshData(V_BB.Data, Indicies);
            }
        }
    }
}
