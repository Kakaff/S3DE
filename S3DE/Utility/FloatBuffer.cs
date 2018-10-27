using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Utility
{
    public sealed class FloatBuffer : IDisposable
    {
        const int SizeIncrement = 64;
        const int InitSize = 16;
        static Queue<FloatBuffer> BuffPool = new Queue<FloatBuffer>();

        float[] data;
        int length;
        bool hasChanged, isDisposed;

        public bool HasChanged => hasChanged;

        public int Length => data.Length;

        public float[] Data {
            get
            {
               if (hasChanged)
                    Apply();
               return data;
            }
        }

        private FloatBuffer()
        {

        }

        public static FloatBuffer Create(int size)
        {
            FloatBuffer b;
            if (BuffPool.Count > 0)
                b = BuffPool.Dequeue();
            else
                b = new FloatBuffer();

            b.Init(size);
            return b;
        }

        public static FloatBuffer Create()
        {
            return Create(InitSize);
        }

        void Resize(int newSize) => Array.Resize(ref data, newSize);

        void ResizeToTarget(int targetSize)
        {
            if (data.Length < targetSize)
                Resize(targetSize);
        }

        public void Clear()
        {
            if (length > InitSize)
            {
                Array.Clear(data, 0, data.Length);
            }
            length = 0;
        }

        public void Add(float b)
        {
            if (data.Length <= length)
                Resize(data.Length + SizeIncrement);

            data[length] = b;

            hasChanged = true;

            length++;
        }

        public void AddRange(params float[] arr)
        {
            ResizeToTarget(length + arr.Length);

            Buffer.BlockCopy(arr, 0, data, length * 4, arr.Length * 4);
            length += arr.Length;

            hasChanged = true;
        }

        void Apply()
        {
            if (length < data.Length)
                Resize(length);

            hasChanged = false;
        }

        void Init(int size)
        {
            if (data == null)
                data = data = new float[InitSize];
            isDisposed = false;
        }

        public void Dispose()
        {
            Return();
        }

        public void Return()
        {
            if (!isDisposed)
            {
                length = 0;
                BuffPool.Enqueue(this);
                isDisposed = true;
            }
        }
    }
}
