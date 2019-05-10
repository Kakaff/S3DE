using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Meshes
{
    public class VertexAttribute
    {
        public uint Index { get; }
        public uint Stride { get; }
        public uint Offset { get; }
        public int Size { get; }
        public GLType GLType { get; }
        public bool Normalized { get; }
        public bool IsEnabled { internal set; get; }

        public VertexAttribute(uint index, int size, GLType type, bool normalized, uint stride, uint offset)
        {
            Index = index;
            Size = size;
            GLType = type;
            Normalized = normalized;
            Stride = stride;
            Offset = offset;
            IsEnabled = false;
        }
    }
}
