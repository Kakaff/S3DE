﻿using System;
using System.Collections.Generic;

namespace S3DE.Utility
{
    public class ByteBuffer : IDisposable
    {
        const int SizeIncrement = 64;
        const int InitSize = 16;
        static Queue<ByteBuffer> BuffPool = new Queue<ByteBuffer>();

        byte[] data;
        int length;
        bool hasChanged,isDisposed;

        public bool HasChanged => hasChanged;


        public byte[] Data
        {
            get
            {
                if (hasChanged)
                    Apply();
                return data;
            }
        }

        public int Length => data.Length;
        

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
                Array.Clear(data, 0, data.Length);
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
                data = data = new byte[InitSize];
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

        public static implicit operator byte[] (ByteBuffer buff) {
            if (buff.HasChanged)
                buff.Apply();
            return buff.data;
        }
    }
}
