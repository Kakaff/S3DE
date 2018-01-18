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
        Vector3[] vertices;
        int[] indicies;
        Vector2[] uvs;
        Vector3[] normals;
        bool dynamic;

        public Vector3[] Vertices { set => vertices = value; get => normals; }
        public Vector3[] Normals { set => normals = value; get => normals; }
        public Vector2[] Uvs { set => uvs = value; get => uvs; }
        public int[] Triangles { set => indicies = value; get => indicies; }
        public bool IsDynamic { get => dynamic; set => dynamic = value;}

        public void CalculateBounds()
        {

        }

        public void ReCalculateNormals()
        {

        }
    }
}
