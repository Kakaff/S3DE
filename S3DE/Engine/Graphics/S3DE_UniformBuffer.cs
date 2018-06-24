using S3DE.Engine.Graphics;
using S3DE.Utility;
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
        public int BoundUniformBlockBindingPoint { get => boundBlockBindingPoint; set => boundBlockBindingPoint = value; }

        public abstract void SetData(byte[] data);

        public void SetData(params byte[][] dataArrays)
        {
            int bytes = dataArrays.Sum(x => x.Length);

            ByteBuffer buff = ByteBuffer.Create(bytes);

            for (int i = 0; i < dataArrays.Length; i++)
                buff.AddRange(dataArrays[i]);

            SetData(buff);
            buff.Dispose();
        }

        public static void BindBuffers(params S3DE_UniformBuffer[] buffers)
        {
            HashSet<int> forbiddenLocations = new HashSet<int>();

            foreach (S3DE_UniformBuffer buff in buffers)
            {
                if (buff.isBound)
                    forbiddenLocations.Add(buff.BoundUniformBlockBindingPoint);
                else
                {
                    buff.Bind(forbiddenLocations);
                    forbiddenLocations.Add(buff.BoundUniformBlockBindingPoint);
                }
            }
        }

        int FindEmptyUniformBindingPoint(HashSet<int> reservedLocations)
        {
            int t;

            if (UnboundUniformBuffers.Count > 0)
                t = UnboundUniformBuffers.Dequeue();
            else
            {
                while (reservedLocations.Contains((t = BoundUniformBuffers.Dequeue()))) ;
                Unbind(t);
                t = UnboundUniformBuffers.Dequeue();
            }

            return t;
        }

        int FindEmptyUniformBindingPoint()
        {
            int t;

            if (UnboundUniformBuffers.Count > 0)
                t = UnboundUniformBuffers.Dequeue();
            else
            {
                t = BoundUniformBuffers.Dequeue();
                Unbind(t);
                t = UnboundUniformBuffers.Dequeue();
            }

            return t;
        }

        public void Bind(HashSet<int> reservedLocations)
        {
            if (!isBound)
            {
                int t = FindEmptyUniformBindingPoint(reservedLocations);

                BoundUniformBuffers.Enqueue(t);
                Renderer.BindUniformBuffer(this, t);
                Buffers[t] = this;
                isBound = true;
                boundBlockBindingPoint = t;
            }
        }

        public void Bind()
        {
            if (!isBound)
            {
                int t = FindEmptyUniformBindingPoint();

                BoundUniformBuffers.Enqueue(t);
                Renderer.BindUniformBuffer(this,t);
                Buffers[t] = this;
                isBound = true;
                boundBlockBindingPoint = t;
            }
        }

        public static void Unbind(int bindingPoint)
        {
            S3DE_UniformBuffer buff = Buffers[bindingPoint];
            if (buff != null)
            {
                UnboundUniformBuffers.Enqueue(bindingPoint);
                buff.isBound = false;
                buff.boundBlockBindingPoint = -1;
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
