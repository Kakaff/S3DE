using System.Collections;
using System.Collections.Generic;

namespace S3DE.Collections
{
    public sealed class DualList<T> : IEnumerable
    {
        List<T> ReadList,WriteList;
        public DualList()
        {
            ReadList = new List<T>();
            WriteList = new List<T>();
        }

        public void Add(T value)
        {
            WriteList.Add(value);
        }

        public void AddRange(T[] values) => WriteList.AddRange(values);
        
        public void Remove(T value)
        {
            ReadList.Remove(value);
        }

        public void Purge(T value)
        {
            ReadList.Remove(value);
            WriteList.Remove(value);
        }

        public void Swap()
        {
            List<T> tmp = WriteList;
            WriteList = ReadList;
            ReadList = tmp;
        }

        public void SwapAndClear()
        {
            Swap();
            WriteList.Clear();
        }

        public T[] ToArray() => ReadList.ToArray();

        public IEnumerator GetEnumerator()
        {
            return ReadList.GetEnumerator();
        }
    }
}
