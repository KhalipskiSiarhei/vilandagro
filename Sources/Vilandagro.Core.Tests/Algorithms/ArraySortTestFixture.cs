using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vilandagro.Core.Algorithms;

namespace Vilandagro.Core.Tests.Algorithms
{
    public abstract class ArraySortTestFixture
    {
        private const int ArraySize = 100;
        protected ISort _sorting;
        private Random _random;
        protected int[] _arrayToSort;

        [SetUp]
        public virtual void SetUp()
        {
            _random = new Random();

            _arrayToSort = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                _arrayToSort[i] = _random.Next(1000000000);
            }
            Console.WriteLine("Array to sort: ");
            Array.ForEach(_arrayToSort, (i) => Console.Write(string.Format("{0},", i)));
        }

        [Test]
        public void SortAsc_ArrayIsUnsorted_ArrayWasSorted()
        {
            // Act
            var sortedArray = _sorting.Sort(_arrayToSort, true).ToArray();

            // Arrange
            for (int i = 0; i < ArraySize - 1; i++)
            {
                Assert.IsTrue(sortedArray[i] <= sortedArray[i + 1]);
                Assert.IsTrue(sortedArray.Contains(_arrayToSort[i]));
            }
            Assert.IsTrue(sortedArray.Contains(_arrayToSort[ArraySize - 1]));
        }

        [Test]
        public void SortDesc_ArrayIsUnsorted_ArrayWasSorted()
        {
            // Act
            var sortedArray = _sorting.Sort(_arrayToSort, false).ToArray();

            // Arrange
            for (int i = 0; i < ArraySize - 1; i++)
            {
                Assert.IsTrue(sortedArray[i] >= sortedArray[i + 1]);
                Assert.IsTrue(sortedArray.Contains(_arrayToSort[i]));
            }
            Assert.IsTrue(sortedArray.Contains(_arrayToSort[ArraySize - 1]));
        }

        [Test]
        public void SortAsc_ArrayIsSortedByDesc_ArrayWasSorted()
        {
            // Arrange
            _arrayToSort = _sorting.Sort(_arrayToSort, false).ToArray();

            // Act
            var sortedArray = _sorting.Sort(_arrayToSort, true).ToArray();

            // Arrange
            for (int i = 0; i < ArraySize - 1; i++)
            {
                Assert.IsTrue(sortedArray[i] <= sortedArray[i + 1]);
                Assert.IsTrue(sortedArray.Contains(_arrayToSort[i]));
            }
            Assert.IsTrue(sortedArray.Contains(_arrayToSort[ArraySize - 1]));
        }

        [Test]
        public void SortDesc_ArrayIsSortedByAsc_ArrayWasSorted()
        {
            // Arrange
            _arrayToSort = _sorting.Sort(_arrayToSort, true).ToArray();

            // Act
            var sortedArray = _sorting.Sort(_arrayToSort, false).ToArray();

            // Arrange
            for (int i = 0; i < ArraySize - 1; i++)
            {
                Assert.IsTrue(sortedArray[i] >= sortedArray[i + 1]);
                Assert.IsTrue(sortedArray.Contains(_arrayToSort[i]));
            }
            Assert.IsTrue(sortedArray.Contains(_arrayToSort[ArraySize - 1]));
        }
    }
}