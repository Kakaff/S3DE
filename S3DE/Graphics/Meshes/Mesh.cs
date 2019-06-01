using S3DE.Maths;
using S3DE.Utility;
using System;
using System.Collections.Generic;

namespace S3DE.Graphics.Meshes
{
    public abstract class Mesh
    {
        S3DECore.Graphics.Mesh internalMesh;
        
        public bool IsBound { private set; get; }
        public bool IsEmpty { private set; get; }
        public bool HasChanged { protected set; get; }

        VertexAttribute[] attributes;

        public Mesh()
        {
            internalMesh = new S3DECore.Graphics.Mesh();
            if (!Renderer.NoError)
                throw new Exception("Error creating mesh!");

            attributes = new VertexAttribute[16];
        }

        protected void SetVertexAttribute(VertexAttribute va)
        {
            if (!internalMesh.IsBound())
                Bind();
            
            internalMesh.SetVertexAttrib(va.Index, va.Size, (uint)va.GLType, (byte)(va.Normalized ? 1 : 0), va.Stride, va.Offset);

            if (!Renderer.NoError)
                throw new Exception("Error setting vertex attribute!");

            attributes[va.Index] = va;
        }
        
        protected void EnableVertexAttribute(uint index)
        {
            VertexAttribute va = attributes[index];
            if (va == null)
                throw new NullReferenceException($"Mesh does not contain a VertexAttribute with index {index}");

            if (va.IsEnabled)
                throw new ArgumentException($"VertexAttribute {index} is already enabled!");

            if (!IsBound)
                Bind();

            internalMesh.EnableVertexAttrib(index);
            if (!Renderer.NoError)
                throw new Exception("Error enabling vertex attribute!");

            attributes[index].IsEnabled = true;
        }

        void Bind()
        {
            internalMesh.Bind();
            if (!Renderer.NoError)
                throw new Exception("Error binding mesh!");
        }

        public void Draw()
        {
            if (!IsBound)
                Bind();

            internalMesh.Draw();
            if (!Renderer.NoError)
                throw new Exception("Error drawing Mesh!");
        }

        protected void UploadMeshData(byte[] vertexData,ushort[] indicies)
        {
            if (!IsBound)
                Bind();

            using (ByteBuffer I_BB = ByteBuffer.Create(indicies.Length * 2))
            {
                I_BB.AddRange(2,indicies);

                unsafe
                {
                    fixed (byte* vb = &vertexData[0])
                        internalMesh.SetVertexData(vb, (uint)vertexData.Length, (uint)BufferUsage.STATIC_DRAW);
                    if (!Renderer.NoError)
                        throw new Exception("Error setting VertexData!");
                    fixed (byte* ib = &I_BB.Data[0])
                        internalMesh.SetIndicies(ib, (uint)I_BB.Data.Length, (uint)BufferUsage.STATIC_DRAW);
                    if (!Renderer.NoError)
                        throw new Exception("Error setting Indicies!");
                }
                IsEmpty = vertexData.Length == 0 && I_BB.Data.Length == 0;

            }
        }

        public abstract void Apply();
        
    }
}
