using System;

namespace S3DE.Collections
{
    //This could easily be done with just a normal list but meh. Writing classes is fun sometimes.
    public class LinkedQueueList<T>
    {
        private class QueueListEntry
        {
            T value;
            int index;

            public int Index {
                get => index;
                internal set => index = value;
            }

            public T Value => value;

            public QueueListEntry Parent => parent;
            public QueueListEntry Child => child;

            QueueListEntry parent, child;

            public QueueListEntry(T val) {
                value = val;
            }

            public QueueListEntry(T val,int index)
            {
                value = val;
                this.index = index;
            }

            public void SetParent(QueueListEntry p) => parent = p;
            public void SetChild(QueueListEntry c) => child = c;
            public void SetIndex(int index) => this.index = index;

            public override bool Equals(object obj)
            {
                if (!(obj is T))
                    return false;
                
                return ((T)obj).Equals(value);
            }
        }

        int length;
        QueueListEntry head,tail;

        public int Count => length;

        public LinkedQueueList()
        {
            length = 0;
        }

        public T Head => head.Value;
        public T Tail => tail.Value;

        public T this[uint index] => ElementAt(index);

        public T ElementAt(uint index)
        {
            if (index > length - 1)
                throw new ArgumentOutOfRangeException();

            uint dstHead, dstTail;
            int tdir;

            dstHead = index;
            dstTail = (uint)length - index;

            QueueListEntry qle;
            if (dstHead <= dstTail)
            {
                qle = head;
                tdir = 1;
            } else
            {
                qle = tail;
                tdir = -1;
            }

            int cIndex = qle.Index;

            while ((uint)cIndex != index)
            {
                if (tdir == -1)
                    qle = qle.Parent;
                else
                    qle = qle.Child;

                cIndex = qle.Index;
            }

            return qle.Value;
        }

        public void Remove(T value)
        {
            QueueListEntry qle = head;
            
            bool vFound = false;

            while (qle != null)
            {
                if (qle.Equals(value))
                {
                    vFound = true;
                    if (qle.Parent != null)
                        qle.Parent.SetChild(qle.Child);
                    else
                        head = qle.Child;

                    if (qle.Child != null)
                    {
                        qle.Child.SetParent(qle.Parent);
                        QueueListEntry qlec = qle.Child;
                        while (qlec != null)
                        {
                            qlec.Index -= 1;
                            qlec = qlec.Child;
                        }
                    }
                    else
                        tail = qle.Parent;
                    
                    length -= 1;
                    return;
                }
                else
                    qle = qle.Child;
                
            }

            if (!vFound)
                throw new Exception($"Unable to remove value {value.ToString()} in linked queuelist");
            
        }

        public T Dequeue()
        {
            QueueListEntry res = head;
            if (res != null)
            {
                head = res.Child;
                QueueListEntry qle = head;

                while (qle != null)
                {
                    qle.Index -= 1;
                    qle = qle.Child;
                }

                if (head != null)
                {
                    res.SetChild(null);
                    head.SetParent(null);
                    length -= 1;
                }
                else
                {
                    length = 0;
                    tail = null;
                    head = null;
                }
                return res.Value;
            }
            else
                throw new InvalidOperationException("Unable to dequeu LinkedQueueList because it is empty");
        }

        public void Enqueue(T val)
        {
            QueueListEntry qle = new QueueListEntry(val);
            if (head == null)
            {
                qle.SetIndex(0);
                head = qle;
                tail = qle;
            } else
            {
                tail.SetChild(qle);
                qle.SetIndex(tail.Index + 1);
                tail = qle;
            }

            length += 1;
        }
    }
}
