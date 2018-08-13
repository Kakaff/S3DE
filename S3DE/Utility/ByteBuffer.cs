using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Utility
{
    public class ByteBuffer : IDisposable
    {
        const int SizeIncrement = 64;
        const int InitSize = 16;
        static ArrayPool<byte> ArrPool = ArrayPool<byte>.Create();
        static Queue<ByteBuffer> BuffPool = new Queue<ByteBuffer>();

        byte[] data;
        int length;
        bool hasChanged,isDisposed;

        public bool HasChanged => hasChanged;

        public int Length => data.Length;

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index > length)
                    throw new ArgumentOutOfRangeException();

                return data[index];
            }

            set
            {
                if (index < 0 || index > length)
                    throw new ArgumentOutOfRangeException();

                data[index] = value;
            }
        }

        private ByteBuffer()
        {
            
        }

        public static ByteBuffer Create(int size)
        {
            ByteBuffer b;
            if (BuffPool.Count > 0)
                b = BuffPool.Dequeue();
            else
                b = new ByteBuffer();
            
            b.Init(size);
            return b;
        }

        public static ByteBuffer Create()
        {
            return Create(InitSize);
        }

        void Resize(int newSize) => Array.Resize(ref data, newSize);

        public void ResizeToTarget(int targetSize)
        {
            if (data.Length < targetSize)
                Resize(targetSize);
        }

        public void Clear() {
            if (length > InitSize)
            {
                ArrPool.Return(data, false);
                data = ArrPool.Rent(InitSize);
            }
            length = 0;
        }

        public void Add(byte b)
        {
            if (data.Length <= length)
                Resize(data.Length + SizeIncrement);

            data[length] = b;

            hasChanged = true;

            length++;
        }

        public void AddRange(params byte[] arr)
        {
            ResizeToTarget(length + arr.Length);

            Buffer.BlockCopy(arr, 0, data, length, arr.Length);
            length += arr.Length;
            
            hasChanged = true;
        }

        public void AddRange<T>(int ByteSize, params T[] values)
        {
            int bytes = values.Length * ByteSize;

            ResizeToTarget(length + bytes);

            Buffer.BlockCopy(values, 0, data, length, bytes);
            length += bytes;
            hasChanged = true;
        }

        public void AddRange<T>(int ByteSize, T[,] values)
        {
            int bytes = ByteSize * values.Length;

            ResizeToTarget(length + bytes);

            Buffer.BlockCopy(values, 0, data, length, bytes);
            length += bytes;
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
                data = ArrPool.Rent(size);
            isDisposed = false;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                length = 0;
                ArrPool.Return(data);
                data = null;
                BuffPool.Enqueue(this);
                isDisposed = true;
            }
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
        
        public static implicit operator byte[] (ByteBuffer buff) {
            if (buff.HasChanged)
                buff.Apply();
            return buff.data;
        }
    }
}
