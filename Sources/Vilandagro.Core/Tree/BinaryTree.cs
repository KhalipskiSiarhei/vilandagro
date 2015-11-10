using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization.Configuration;

namespace Vilandagro.Core.Tree
{
    public class BinaryTree<T> where T : struct
    {
        protected Node<T> _rootNode;

        public BinaryTree()
        {
            _rootNode = null;
        }

        public BinaryTree(T rootValue)
        {
            _rootNode = new Node<T>() { Value = rootValue };
        }

        public Node<T> RootNode
        {
            get { return _rootNode; }
            set { _rootNode = value; }
        }

        /// <summary>
        /// It is DFS algorithm implementation to add new elements
        /// </summary>
        /// <param name="value"></param>
        public virtual void Add(T value)
        {
            if (_rootNode == null)
            {
                _rootNode = new Node<T>() { Value = value };
            }
            else
            {
                var queue = new Queue<Node<T>>();

                queue.Enqueue(_rootNode);
                while (queue.Count > 0)
                {
                    var node = queue.Dequeue();

                    if (node.Left != null)
                    {
                        queue.Enqueue(node.Left);
                    }
                    else
                    {
                        node.Left = new Node<T>() {Value = value, Parent = node};
                        return;
                    }

                    if (node.Right != null)
                    {
                        queue.Enqueue(node.Right);
                    }
                    else
                    {
                        node.Right = new Node<T>() { Value = value, Parent = node };
                        return;
                    }
                }
            }
        }

        public bool IsBalanced()
        {
            var result = CheckTreeByBalace(_rootNode, 0);
            return result.IsTreeBalanced;
        }

        public bool IsSearch()
        {
            return CheckTreeBySearch(_rootNode, null, null);
        }

        public Nullable<T> GetFirstCommonAncestor(T value1, T value2)
        {
            var node1 = SearchNodeByValue(_rootNode, value1);

            if (node1 != null)
            {
                var nodeToSearchFrom = node1.Parent;

                while (nodeToSearchFrom != null)
                {
                    var node2 = SearchNodeByValue(nodeToSearchFrom, value2);

                    if (node2 != null && node2 != nodeToSearchFrom && node2 != node1)
                    {
                        return nodeToSearchFrom.Value;
                    }
                    nodeToSearchFrom = nodeToSearchFrom.Parent;
                }
            }

            return null;
        }

        private Node<T> SearchNodeByValue(Node<T> node, T value)
        {
            var comparer = Comparer<T>.Default;

            if (comparer.Compare(node.Value, value) == 0)
            {
                return node;
            }
            if (node.Left != null)
            {
                var searchedNode = SearchNodeByValue(node.Left, value);

                if (searchedNode != null)
                {
                    return searchedNode;
                }
            }
            if (node.Right != null)
            {
                var searchedNode = SearchNodeByValue(node.Right, value);

                if (searchedNode != null)
                {
                    return searchedNode;
                }
            }
            return null;
        }

        private TreeBalancedResult CheckTreeByBalace(Node<T> node, int currentHeight)
        {
            var leftSubTreeResult = node.Left != null
                ? CheckTreeByBalace(node.Left, currentHeight + 1)
                : new TreeBalancedResult() { IsTreeBalanced = true, TreeHeight = currentHeight };
            if (!leftSubTreeResult.IsTreeBalanced)
            {
                return leftSubTreeResult;
            }

            var rightSubTreeResult = node.Right != null
                ? CheckTreeByBalace(node.Right, currentHeight + 1)
                : new TreeBalancedResult() { IsTreeBalanced = true, TreeHeight = currentHeight };
            if (!rightSubTreeResult.IsTreeBalanced)
            {
                return rightSubTreeResult;
            }

            var subTreeHeightDifference = leftSubTreeResult.TreeHeight - rightSubTreeResult.TreeHeight;
            return new TreeBalancedResult()
            {
                IsTreeBalanced = subTreeHeightDifference >= -1 && subTreeHeightDifference <= 1,
                TreeHeight = Math.Max(leftSubTreeResult.TreeHeight.Value, rightSubTreeResult.TreeHeight.Value)
            };
        }

        private bool CheckTreeBySearch(Node<T> node, Nullable<T> from, Nullable<T> to)
        {
            if (node.Left != null)
            {
                return CheckTreeBySearch(node.Left, from, node.Value);
            }

            var comparer = Comparer<T>.Default;
            if ((from.HasValue && comparer.Compare(from.Value, node.Value) > 0) ||
                (to.HasValue && comparer.Compare(node.Value, to.Value) > 0))
            {
                return false;
            }

            if (node.Right != null)
            {
                return CheckTreeBySearch(node.Right, node.Value, to);
            }

            return true;
        }
    }
}