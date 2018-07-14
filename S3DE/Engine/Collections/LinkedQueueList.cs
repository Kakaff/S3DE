﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Collections
{
    //This could easily be done with just a normal list but meh. Writing classes is fun sometimes.
    public class LinkedQueueList<T>
    {
        private class QueueListEntry
        {
            T value;
            int index;

            public int Index => index;
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
        }

        int length;
        QueueListEntry head,tail;

        public int Count => length;

        public LinkedQueueList()
        {
            length = 0;
        }

        public void Remove(T value)
        {
            QueueListEntry qle = head;

            while (qle != null)
            {
                if (qle.Equals(value))
                {
                    if (qle.Parent != null)
                        qle.Parent.SetChild(qle.Child);
                    if (qle.Child != null)
                        qle.Child.SetParent(qle.Parent);

                    if (head.Equals(qle))
                        head = qle.Child;
                    if (tail.Equals(qle))
                        tail = qle.Parent;

                    length -= 1;
                    return;
                }
                else
                    qle = qle.Child;
                
            }
        }

        public T Dequeue()
        {
            QueueListEntry res = head;
            if (res != null)
            {
                head = res.Child;
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
