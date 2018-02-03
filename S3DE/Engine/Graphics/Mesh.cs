using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public sealed class Mesh
    {
        public Mesh()
        {
            vertices = new Vector3[0];
            indicies = new int[0];
            uvs = new Vector2[0];
            normals = new Vector3[0];
            dynamic = false;
        }

        Vector3[] vertices;
        int[] indicies;
        Vector2[] uvs;
        Vector3[] normals;
        bool dynamic;

        public Vector3[] Vertices { set => vertices = value; get => vertices; }
        public Vector3[] Normals { set => normals = value; get => normals; }
        public Vector2[] Uvs { set => uvs = value; get => uvs; }
        public int[] Triangles { set => indicies = value; get => indicies; }
        public bool IsDynamic { get => dynamic; set => dynamic = value;}

        public void CalculateBounds()
        {

        }

        public void ReCalculateNormals()
        {
            Vector3[] newNormals = new Vector3[vertices.Length];

            for (int i = 0; i < indicies.Length; i+= 3)
            {
                int i0 = indicies[i];
                int i1 = indicies[i + 1];
                int i2 = indicies[i + 2];

                Vector3 v1 = vertices[i1] - vertices[0];
                Vector3 v2 = vertices[i2] - vertices[0];

                Vector3 norm = v1.Cross(v2).normalized;

                newNormals[i0] += norm;
                newNormals[i1] += norm;
                newNormals[i2] += norm;
            }

            for (int i = 0; i < newNormals.Length; i++)
                newNormals[i] = newNormals[i].normalized;

            Normals = newNormals;
        }

        public static Mesh CreateCube(Vector3 scale)
        {
            Mesh m = new Mesh();

            float x = 0.5f * scale.x;
            float y = 0.5f * scale.y;
            float z = 0.5f * scale.z;

            Vector3[] verts = new Vector3[] {new Vector3(-x,y,-z), new Vector3(x,y,-z),new Vector3(x,-y,-z), new Vector3(-x,-y,-z)
                                            ,new Vector3(x,y,z), new Vector3(-x,y,z),new Vector3(-x,-y,z), new Vector3(x,-y,z)};

            Vector2[] uvs = new Vector2[] {Vector2.Zero,Vector2.Zero,Vector2.Zero,Vector2.Zero,
                                           Vector2.One,Vector2.One,Vector2.One,Vector2.One};

            int[] triangles = new int[] {0,1,2, //zNeg 1
                                         0,2,3, //zNeg 2
                                         4,5,6, //zpos 1
                                         4,6,7,  //zpos 2
                                         1,4,7, //x pos 1
                                         1,7,2, //x pos 2
                                         5,0,3, //x neg 1
                                         5,3,6, //x neg 2
                                         5,4,1, // y pos 1
                                         5,1,0, // y pos 2
                                         3,2,7,
                                         3,7,6
                                };

            m.Vertices = verts;
            m.Uvs = uvs;
            m.Triangles = triangles;
            m.ReCalculateNormals();
            return m;
        }
    }
}
