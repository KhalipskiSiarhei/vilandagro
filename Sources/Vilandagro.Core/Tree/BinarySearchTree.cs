using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Core.Tree
{
    public class BinarySearchTree<T> : BinaryTree<T> where T : struct
    {
        public BinarySearchTree()
            : base()
        {
        }

        public BinarySearchTree(T rootValue)
            : base(rootValue)
        {
        }

        public override void Add(T value)
        {
            if (_rootNode == null)
            {
                _rootNode = new Node<T>() { Value = value };
            }
            else
            {
                CreateLeafNode(_rootNode, value);
            }
        }

        private Node<T> CreateLeafNode(Node<T> parent, T value)
        {
            var comparer = Comparer<T>.Default;

            if (parent.Left != null || parent.Right != null)
            {
                if (comparer.Compare(parent.Value, value) >= 0)
                {
                    if (parent.Left != null)
                    {
                        return CreateLeafNode(parent.Left, value);
                    }
                }
                else
                {
                    if (parent.Right != null)
                    {
                        return CreateLeafNode(parent.Right, value);
                    }
                }
            }

            var leafNode = new Node<T>() { Value = value, Parent = parent };

            if (comparer.Compare(parent.Value, value) >= 0)
            {
                parent.Left = leafNode;
            }
            else
            {
                parent.Right = leafNode;
            }
            return leafNode;
        }
    }
}
