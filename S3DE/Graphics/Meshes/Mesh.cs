using S3DE.Maths;
using S3DE.Utility;
using System;
using System.Collections.Generic;

namespace S3DE.Graphics.Meshes
{
    public abstract partial class Mesh
    {
        static Mesh activeMesh;

        IntPtr handle;
        
        public bool IsBound { private set; get; }
        public bool IsEmpty { private set; get; }
        public bool HasChanged { protected set; get; }

        VertexAttribute[] attributes;

        public Mesh()
        {
            handle = CreateMesh();
            attributes = new VertexAttribute[16];
        }

        protected void SetVertexAttribute(VertexAttribute va)
        {
            if (!IsBound)
                Bind();
           
            SetVertexAttrib(handle, va.Index, va.Size, va.GLType, va.Normalized, va.Stride, va.Offset);

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
            
            EnableVertexAttrib(handle, index);
            if (!Renderer.NoError)
                throw new Exception("Error enabling vertex attribute!");

            attributes[index].IsEnabled = true;
        }

        void Bind()
        {
            if (!IsBound)
            {
                if (activeMesh != null)
                    activeMesh.IsBound = false;

                Extern_Mesh_Bind(handle);
                activeMesh = this;
                IsBound = true;
            }
        }

        public void Draw()
        {
            if (!IsBound)
                 Bind();

            DrawMesh(handle);
        }

        protected void UploadMeshData(byte[] vertexData,ushort[] indicies)
        {
            if (!IsBound)
                Bind();

            using (ByteBuffer I_BB = ByteBuffer.Create(indicies.Length * 2))
            {
                I_BB.AddRange(2,indicies);

                using (PinnedMemory vPM = new PinnedMemory(vertexData))
                    using (PinnedMemory iPM = new PinnedMemory(I_BB.Data))
                {
                    SetMeshData(handle, 
                        vertexData, (uint)vertexData.Length,
                        I_BB.Data, (uint)I_BB.Data.Length,
                        BufferUsage.STATIC_DRAW);

                    IsEmpty = vertexData.Length == 0 && I_BB.Data.Length == 0;
                }
            }
        }

        public abstract void Apply();
        
    }
}
