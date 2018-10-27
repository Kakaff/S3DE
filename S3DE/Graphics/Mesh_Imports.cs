using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics
{
    partial class Mesh
    {
        [DllImport("S3DECore.dll")]
        private static extern void SetVertexAttrib(IntPtr mesh, uint index,
            int size, GLType type, bool normalized, uint stride, uint offset);

        [DllImport("S3DECore.dll")]
        private static extern void EnableVertexAttrib(IntPtr mesh, uint index);

        [DllImport("S3DECore.dll")]
        private static extern void DrawMesh(IntPtr mesh);

        [DllImport("S3DECore.dll")]
        private static extern IntPtr CreateMesh();

        [DllImport("S3DECore.dll")]
        private static extern void SetMeshData(IntPtr mesh, byte[] vertices, uint vertcount, byte[] indicies, uint indCount, BufferUsage usage);

    }
}
