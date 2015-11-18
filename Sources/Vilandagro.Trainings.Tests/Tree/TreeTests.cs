using NUnit.Framework;
using Vilandagro.Trainings.Tree;

namespace Vilandagro.Trainings.Tests.Tree
{
    public class TreeTests
    {
        private BinaryTree<int> _tree;

        [TestCase(new[] { 200, 100, 300 })]
        [TestCase(new[] { 50, 45, 55, 40, 48, 53, 60, 38, 42, 47, 49, 52, 54, 58, 61 })]
        public void SearchTree_TreeIsSearch(int[] nodes)
        {
            InitSearchTree(nodes);
            Assert.IsTrue(_tree.IsSearch());
        }

        [TestCase(new[] { 100, 200, 300 })]
        [TestCase(new[] { 50, 51, 52, 53, 54, 55, 56, 57 })]
        public void TreeIsNotSearch_TreeIsNotSearch(int[] nodes)
        {
            InitTree(nodes);
            Assert.IsFalse(_tree.IsSearch());
        }

        [TestCase(new [] {200, 100, 300})]
        [TestCase(new[] { 50, 45, 55, 40, 48, 53, 60, 38, 42, 47, 49, 52, 54, 58, 61 })]
        public void SearchTreeIsBalanced_TreeIsBalanced(int[] nodes)
        {
            InitSearchTree(nodes);
            Assert.IsTrue(_tree.IsBalanced());
        }

        [TestCase(new [] {100, 200, 300})]
        [TestCase(new[] { 50, 45, 55, 40, 48, 53, 60, 41, 42, 47, 49, 52, 54, 58, 61 })]
        public void SearchTreeIsNotBalanced_TreeIsNotBalanced(int[] nodes)
        {
            InitSearchTree(nodes);

            Assert.IsFalse(_tree.IsBalanced());
        }

        [TestCase(new[] { 50, 51, 52, 53, 54, 55, 56, 57 })]
        public void TreeIsBalanced_TreeIsBalanced(int[] nodes)
        {
            InitTree(nodes);
            Assert.IsTrue(_tree.IsBalanced());
        }

        [TestCase(new[] { 100, 200, 300 })]
        [TestCase(new[] { 50, 45, 55, 40, 48, 53, 60, 41, 42, 47, 49, 52, 54, 58, 61 })]
        public void GetFirstCommonAncestor_NoCommonAncestors_NullReturned(int[] nodes)
        {
            InitTree(nodes);
            Assert.IsNull(_tree.GetFirstCommonAncestor(nodes[nodes.Length/2], 45678));

            InitSearchTree(nodes);
            Assert.IsNull(_tree.GetFirstCommonAncestor(nodes[nodes.Length / 2], 45678));
        }

        [TestCase(new[] { 50, 51, 52, 53, 54, 55, 56, 57 })]
        public void GetFirstCommonAncestor_ThereIsCommonAncestorInTree_ValueReturned(int[] nodes)
        {
            InitTree(nodes);

            Assert.IsTrue(_tree.GetFirstCommonAncestor(51, 52) == 50);
            Assert.IsTrue(_tree.GetFirstCommonAncestor(57, 54) == 51);
            Assert.IsTrue(_tree.GetFirstCommonAncestor(57, 53) == 51);
            Assert.IsTrue(_tree.GetFirstCommonAncestor(54, 52) == 50);
            Assert.IsTrue(_tree.GetFirstCommonAncestor(52, 55) == 50);
        }

        [TestCase(new[] { 50, 45, 55, 40, 48, 53, 60, 41, 42, 47, 49, 52, 54, 58, 61 })]
        public void GetFirstCommonAncestor_ThereIsCommonAncestorInSearchTree_ValueReturned(int[] nodes)
        {
            InitSearchTree(nodes);

            Assert.IsTrue(_tree.GetFirstCommonAncestor(48, 53) == 50);
            Assert.IsTrue(_tree.GetFirstCommonAncestor(42, 47) == 45);
            Assert.IsTrue(_tree.GetFirstCommonAncestor(41, 42) == 40);
            Assert.IsTrue(_tree.GetFirstCommonAncestor(47, 52) == 50);
            Assert.IsTrue(_tree.GetFirstCommonAncestor(54, 61) == 55);
        }

        private void InitSearchTree(int[] nodes)
        {
            _tree = new BinarySearchTree<int>();

            foreach (var node in nodes)
            {
                _tree.Add(node);
            }
        }

        private void InitTree(int[] nodes)
        {
            _tree = new BinaryTree<int>();

            foreach (var node in nodes)
            {
                _tree.Add(node);
            }
        }
    }
}