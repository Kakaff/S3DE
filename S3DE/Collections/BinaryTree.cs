using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Collections
{
    //Lacks a remove method and a ToArray method.
    //Being able to iterate over it would also be nice.
    public sealed class BinaryTree<TKey,TValue>
    {

        Node rootNode;

        public void Add(TKey key, TValue value)
        {
            if (rootNode == null)
            {
                rootNode = new Node(key, value);
                return;
            }

            int hash = key.GetHashCode();
            Node n = rootNode;

            while (true)
            {
                if (hash < n.Hash)
                {
                    if (n.Left == null)
                    {
                        n.Left = new Node(key, value);
                        break;
                    }
                    else
                        n = n.Left;
                }
                else if (hash > n.Hash)
                {
                    if (n.Right == null)
                    {
                        n.Right = new Node(key, value);
                        break;
                    }
                    else
                        n = n.Right;
                }
                else
                    throw new ArgumentException($"Key already exists!");
            }
        }

        public bool TryGet(TKey key, out TValue value)
        {
            Node n = rootNode;
            int hash = key.GetHashCode();
            
            while (n != null)
            {
                if (n.Hash == hash)
                    break;
                else if (hash > n.Hash)
                    n = n.Right;
                else if (hash < n.Hash)
                    n = n.Left;
            }

            value = (n == null) ? default : n.Value;
            return n == null;
        }

        class Node
        {
            public int Hash => hash;
            public TValue Value => val;
            public Node Right { get; set; }
            public Node Left { get; set; }

            int hash;
            TValue val;

            private Node() { }

            public Node(TKey key, TValue value)
            {
                val = value;
                hash = key.GetHashCode();
            }
        }
    }
}
