using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vilandagro.Core.Algorithms;

namespace Vilandagro.Core.Tests.Algorithms
{
    public abstract class StringSortTestFixture
    {
        private const string Chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890qwertyuiop[]asdfghjkl;'zxcvbnm,./\ ";
        private Random _random;

        protected ISort _sorting;
        protected string _toSort;

        [SetUp]
        public virtual void SetUp()
        {
            _random = new Random();
            _toSort = GetRandomString(100);

            Console.WriteLine("String to sort: '{0}'", _toSort);
        }

        [Test]
        public void SortAsc_ArrayIsUnsorted_ArrayWasSorted()
        {
            // Act
            var sorted = _sorting.Sort(_toSort, true);

            // Asserts
            AssertSortedString(_toSort, sorted, true);
        }

        [Test]
        public void SortDesc_ArrayIsUnsorted_ArrayWasSorted()
        {
            // Act
            var sorted = _sorting.Sort(_toSort, false);

            // Asserts
            AssertSortedString(_toSort, sorted, false);
        }

        [Test]
        public void SortAsc_ArrayIsSortedByDesc_ArrayWasSorted()
        {
            // Arrange
            _toSort = _sorting.Sort(_toSort, false);

            // Act
            var sorted = _sorting.Sort(_toSort, true);

            // Asserts
            AssertSortedString(_toSort, sorted, true);
        }

        [Test]
        public void SortDesc_ArrayIsSortedByAsc_ArrayWasSorted()
        {
            // Arrange
            _toSort = _sorting.Sort(_toSort, true);

            // Act
            var sorted = _sorting.Sort(_toSort, false);

            // Asserts
            AssertSortedString(_toSort, sorted, false);
        }

        protected void AssertSortedString(string toSort, string sorted, bool order)
        {
            for (int i = 0; i < toSort.Length - 1; i++)
            {
                if (order)
                {
                    Assert.IsTrue(sorted[i] <= sorted[i + 1]);
                }
                else
                {
                    Assert.IsTrue(sorted[i] >= sorted[i + 1]);
                }
            }
            Assert.IsTrue(sorted.Contains(toSort[toSort.Length - 1]));
        }

        protected string GetRandomString(int size)
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
