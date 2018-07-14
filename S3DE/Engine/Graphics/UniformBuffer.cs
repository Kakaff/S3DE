using S3DE.Engine.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public class UniformBuffer
    {
        uint identifier;
        bool isBound;
        int boundBindingPoint;

        public uint Identifier => identifier;
        public bool IsBound => isBound;
        
        public int BoundUniformBlockBindingPoint { get => boundBindingPoint;}
        public void Bind() => UniformBuffers.Bind(this);

        public static void Bind(params UniformBuffer[] buffers) => UniformBuffers.BindBuffers(buffers); 

        internal UniformBuffer(uint id)
        {
            identifier = id;
            isBound = false;
            boundBindingPoint = -1;
        }

        internal void SetIsBound(bool val,int bindingPoint)
        {
            boundBindingPoint = (val) ? bindingPoint : -1;
            isBound = val;
        }

        public void SetData(byte[] data) => Renderer.SetUniformBufferData(this,data);
    }

    internal static class UniformBuffers
    {
        static UniformBuffer[] Buffers;
        static LinkedQueueList<int> UnboundUniformBuffers, BoundUniformBuffers;

        public static void BindBuffers(params UniformBuffer[] buffers)
        {
            HashSet<int> forbiddenLocations = new HashSet<int>();

            foreach (UniformBuffer buff in buffers)
            {
                if (buff.IsBound)
                    forbiddenLocations.Add(buff.BoundUniformBlockBindingPoint);
                else
                {
                    Bind(buff,forbiddenLocations);
                    forbiddenLocations.Add(buff.BoundUniformBlockBindingPoint);
                }
            }
        }

        static int FindEmptyUniformBindingPoint(HashSet<int> reservedLocations)
        {
            int t;

            if (UnboundUniformBuffers.Count > 0)
                t = UnboundUniformBuffers.Dequeue();
            else
            {
                while (reservedLocations.Contains((t = BoundUniformBuffers.Dequeue()))) { BoundUniformBuffers.Enqueue(t); }
                Unbind(t);
                t = UnboundUniformBuffers.Dequeue();
            }

            return t;
        }

        static int FindEmptyUniformBindingPoint()
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

        public static void Bind(UniformBuffer buffer, HashSet<int> reservedLocations)
        {
            if (!buffer.IsBound)
            {
                int t = FindEmptyUniformBindingPoint(reservedLocations);

                BoundUniformBuffers.Enqueue(t);
                Renderer.BindUniformBuffer(buffer, t);
                buffer.SetIsBound(true, t);
                Buffers[t] = buffer;
            }
        }

        public static void Bind(UniformBuffer buff)
        {
            if (!buff.IsBound)
            {
                int t = FindEmptyUniformBindingPoint();

                BoundUniformBuffers.Enqueue(t);
                Renderer.BindUniformBuffer(buff, t);
                buff.SetIsBound(true, t);
                Buffers[t] = buff;
            }
        }

        public static void Unbind(int bindingPoint)
        {
            UniformBuffer buff = Buffers[bindingPoint];
            if (buff != null)
            {
                UnboundUniformBuffers.Enqueue(bindingPoint);
                buff.SetIsBound(false, -1);
                Buffers[bindingPoint] = null;
            }
        }


        internal static void Initialize_BindingPoints()
        {
            Buffers = new UniformBuffer[Renderer.UniformBlockBindingPoints];

            UnboundUniformBuffers = new LinkedQueueList<int>();
            BoundUniformBuffers = new LinkedQueueList<int>();

            for (int i = 0; i < Renderer.UniformBlockBindingPoints; i++)
                UnboundUniformBuffers.Enqueue((int)i);
        }
    }
}
