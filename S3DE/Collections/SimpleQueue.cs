using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Collections
{
    //This was faster than the System.Collections.Queue, could still use some tweaking though.
    public class SimpleQueue<T>
    {
        T[] values;
        int dequeueIndex, emptyIndex;
        int gen, genSize;

        public int Count { get; private set; }

        public SimpleQueue()
        {
            values = new T[32];
            gen = 0;
            genSize = 16;
        }

        //If there isn't this amount of available space after restructuring the queue
        //Increase it in size instead.
        int minRestructure = 8;

        public void Enqueue(T value)
        {
            //The Queue is "full" so we need to either restructure it or resize it.
            if (emptyIndex == values.Length)
            {
                //Check if we can restructure.
                if (dequeueIndex > minRestructure)
                {
                    Restructure();
                }
                else
                {
                    Resize();
                    Restructure();
                }
            }

            values[emptyIndex] = value;
            emptyIndex++;
            Count++;
        }

        public T Dequeue()
        {
            if (Count == 0)
                throw new System.InvalidOperationException("Queue is empty!");
            dequeueIndex++;
            Count--;
            return values[dequeueIndex - 1];
        }

        void Restructure()
        {
            if (Count > 0)
                System.Array.Copy(values, dequeueIndex, values, 0, Count);

            emptyIndex = Count;
            dequeueIndex = 0;
        }

        void Resize()
        {
            gen++;
            System.Array.Resize(ref values, values.Length + (gen * genSize));
        }
    }
}
