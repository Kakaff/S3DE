using S3DE.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Data
{
    public abstract class S3DE_UniformBuffer
    {

        static S3DE_UniformBuffer[] Buffers;
        static LinkedQueueList<int> UnboundUniformBuffers, BoundUniformBuffers;

        private int boundBlockBindingPoint;
        private bool isBound;

        public abstract uint Identifier { get; }
        public bool IsBound { get => isBound; private set => isBound = value; }
        public int BoundUniformBlockBinding { get => boundBlockBindingPoint; set => boundBlockBindingPoint = value; }

        public abstract void SetData(byte[] data);

        public void Bind()
        {
            if (!isBound)
            {
                int t;

                if (UnboundUniformBuffers.Count > 0)
                    t = UnboundUniformBuffers.Dequeu();
                else
                {
                    t = BoundUniformBuffers.Dequeu();
                    Buffers[t].isBound = false;
                    Buffers[t].boundBlockBindingPoint = -1;
                }

                Renderer.BindUniformBuffer(this,t);
                BoundUniformBuffers.Enqueue(t);
                isBound = true;
                boundBlockBindingPoint = t;

                Buffers[t] = this;
            }
        }

        internal static void Initialize_BindingPoints()
        {
            Buffers = new S3DE_UniformBuffer[Renderer.UniformBlockBindingPoints];
            UnboundUniformBuffers = new LinkedQueueList<int>();
            BoundUniformBuffers = new LinkedQueueList<int>();

            for (int i = 0; i < Renderer.UniformBlockBindingPoints; i++)
                UnboundUniformBuffers.Enqueue((int)i);
        }

        public static S3DE_UniformBuffer Create() => Renderer.CreateUniformBuffer();


        protected S3DE_UniformBuffer() { }
    }
}
