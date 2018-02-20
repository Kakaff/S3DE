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

        int[] indicies;
        Vector2[] uvs;
        Vector3[] vertices;
        Vector3[] normals;
        Vector4[] tangents;
        bool dynamic;

        public Vector4[] Tangents { set => tangents = value; get => tangents;}
        public Vector3[] Vertices { set => vertices = value; get => vertices;}
        public Vector3[] Normals { set => normals = value; get => normals;}
        public Vector2[] Uvs { set => uvs = value; get => uvs;}
        public int[] Indicies { set => indicies = value; get => indicies;}
        public bool IsDynamic { get => dynamic; set => dynamic = value;}

        public void CalculateBounds()
        {

        }

        public void ReCalculateNormals(bool CalculateTangents)
        {
            Vector3[] newNormals = new Vector3[vertices.Length];
            Vector4[] newTangents = null;
            Vector3[] tan1 = null, tan2 = null;

            if (CalculateTangents && uvs.Length != vertices.Length)
                throw new ArgumentException("The mesh needs to have the same number of uvs as vertices to calculate tangents!");
            else if (CalculateTangents)
            {
                tan1 = new Vector3[vertices.Length];
                tan2 = new Vector3[vertices.Length];
                newTangents = new Vector4[vertices.Length];
            }

            for (int i = 0; i < indicies.Length; i+= 3)
            {
                int i0 = indicies[i];
                int i1 = indicies[i + 1];
                int i2 = indicies[i + 2];

                Vector3 v0 = vertices[i0];
                Vector3 v1 = vertices[i1];
                Vector3 v2 = vertices[i2];

                Vector3 n0 = v1 - v0;
                Vector3 n1 = v2 - v0;

                Vector3 norm = n0.Cross(n1).normalized;

                newNormals[i0] += norm;
                newNormals[i1] += norm;
                newNormals[i2] += norm;

                if (CalculateTangents)
                {
                    //Adapted from: Lengyel, Eric. 
                    //“Computing Tangent Space Basis Vectors for an Arbitrary Mesh”. 
                    //Terathon Software, 2001.
                    //http://terathon.com/code/tangent.html

                    Vector2 uv0 = uvs[i0];
                    Vector2 uv1 = uvs[i1];
                    Vector2 uv2 = uvs[i2];

                    Vector2 dUv0 = uv1 - uv0; 
                    Vector2 dUv1 = uv2 - uv0; 

                    float r = 1.0f / (dUv0.x * dUv1.y - dUv1.x * dUv0.y);

                    Vector3 sDir = new Vector3(
                        dUv1.y * n0.x - dUv0.y * n1.x,
                        dUv1.y * n0.y - dUv0.y * n1.y,
                        dUv1.y * n0.z - dUv0.y * n1.z) * r;

                    Vector3 tDir = new Vector3(
                        dUv0.x * n1.x - dUv1.x * n0.x,
                        dUv0.x * n1.y - dUv1.x * n0.y,
                        dUv0.x * n1.z - dUv1.x * n0.z) * r;

                    tan1[i0] += sDir;
                    tan1[i1] += sDir;
                    tan1[i2] += sDir;

                    tan2[i0] += tDir;
                    tan2[i1] += tDir;
                    tan2[i2] += tDir;
                }
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                newNormals[i] = newNormals[i].normalized;
                if (CalculateTangents)
                {
                    Vector3 norm = newNormals[i];
                    Vector3 tan = tan1[i];

                    newTangents[i] = (tan - norm * Vector3.Dot(norm, tan)).normalized;
                    newTangents[i].w = (Vector3.Dot(Vector3.Cross(norm, tan), tan2[i]) < 0.0f) ? -1 : 1;
                }
            }

            Normals = newNormals;
            if (CalculateTangents)
            {
                Tangents = newTangents;
                tan1 = null;
                tan2 = null;
            }
        }

        public static Mesh CreateCube(Vector3 scale)
        {
            Mesh m = new Mesh();

            float x = 0.5f * scale.x;
            float y = 0.5f * scale.y;
            float z = 0.5f * scale.z;
            Vector3[] verts;
            Vector2[] uvs;

            int[] triangles;
            
                verts = new Vector3[] {new Vector3(-x,y,-z),new Vector3(x,y,-z), new Vector3(x,-y,-z), new Vector3(-x,-y,-z),  //zNeg
                                       new Vector3(-x,y,z), new Vector3(-x,y,-z), new Vector3(-x,-y,-z), new Vector3(-x,-y,z), //xneg
                                       new Vector3(x,y,-z),new Vector3(x,y,z), new Vector3(x,-y,z),new Vector3(x,-y,-z),       //xPos
                                       new Vector3(x,y,z), new Vector3(-x,y,z), new Vector3(-x,-y,z), new Vector3(x,-y,z),     //zPos
                                       new Vector3(-x,y,z), new Vector3(x,y,z), new Vector3(x,y,-z), new Vector3(-x,y,-z),
                                       new Vector3(-x,-y,-z),new Vector3(x,-y,-z), new Vector3(x,-y,z), new Vector3(-x,-y,z)};

                uvs = new Vector2[] {new Vector2(0,1),new Vector2(1,1),new Vector2(1,0), new Vector2(0,0),
                                     new Vector2(0,1),new Vector2(1,1),new Vector2(1,0), new Vector2(0,0),
                                     new Vector2(0,1),new Vector2(1,1),new Vector2(1,0), new Vector2(0,0),
                                     new Vector2(0,1),new Vector2(1,1),new Vector2(1,0), new Vector2(0,0),
                                     new Vector2(0,1),new Vector2(1,1),new Vector2(1,0), new Vector2(0,0),
                                     new Vector2(0,1),new Vector2(1,1),new Vector2(1,0), new Vector2(0,0),};
                
                triangles = new int[] {0,1,2,
                                       0,2,3,
                                       4,5,6,
                                       4,6,7,
                                       8,9,10,
                                       8,10,11,
                                       12,13,14,
                                       12,14,15,
                                       16,17,18,
                                       16,18,19,
                                       20,21,22,
                                       20,22,23};
            
            m.Vertices = verts;
            m.Uvs = uvs;
            m.Indicies = triangles;
            m.ReCalculateNormals(true);

            return m;
        }
    }
}
