using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vilandagro.Core.Algorithms;

namespace Vilandagro.Core.Tests.Algorithms
{
    [TestFixture]
    public class StringInsertionSortTests
    {
        private const string Chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890qwertyuiop[]asdfghjkl;'zxcvbnm,./\";
        private const int StringSize = 100;
        private InsertionSort _sorting;
        private Random _random;
        private string _stringToSort;

        [SetUp]
        public void SetUp()
        {
            _sorting = new InsertionSort();
            _random = new Random();

            _stringToSort = GetRandomString(StringSize);
        }

        [Test]
        public void SortAsc_ArrayIsUnsorted_ArrayWasSorted()
        {
            // Act
            var sortedString = _sorting.Sort(_stringToSort, true).ToArray();

            // Arrange
            for (int i = 0; i < StringSize - 1; i++)
            {
                Assert.IsTrue(sortedString[i] <= sortedString[i + 1]);
            }
        }

        [Test]
        public void SortDesc_ArrayIsUnsorted_ArrayWasSorted()
        {
            // Act
            var sortedString = _sorting.Sort(_stringToSort, false).ToArray();

            // Arrange
            for (int i = 0; i < StringSize - 1; i++)
            {
                Assert.IsTrue(sortedString[i] >= sortedString[i + 1]);
            }
        }

        [Test]
        public void SortAsc_ArrayIsSortedByDesc_ArrayWasSorted()
        {
            // Arrange
            _stringToSort = _sorting.Sort(_stringToSort, false);

            // Act
            var sortedString = _sorting.Sort(_stringToSort, true).ToArray();

            // Arrange
            for (int i = 0; i < StringSize - 1; i++)
            {
                Assert.IsTrue(sortedString[i] <= sortedString[i + 1]);
            }
        }

        [Test]
        public void SortDesc_ArrayIsSortedByAsc_ArrayWasSorted()
        {
            // Arrange
            _stringToSort = _sorting.Sort(_stringToSort, true);

            // Act
            var sortedArray = _sorting.Sort(_stringToSort, false).ToArray();

            // Arrange
            for (int i = 0; i < StringSize - 1; i++)
            {
                Assert.IsTrue(sortedArray[i] >= sortedArray[i + 1]);
            }
        }
       
        private string GetRandomString(int size)
        {
            var buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = Chars[_random.Next(Chars.Length)];
            }
            return new string(buffer);
        }
    }
}