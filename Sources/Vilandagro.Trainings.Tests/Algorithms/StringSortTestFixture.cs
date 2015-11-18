using System;
using System.Linq;
using NUnit.Framework;
using Vilandagro.Trainings.Algorithms;

namespace Vilandagro.Trainings.Tests.Algorithms
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
        }

        [Test]
        public void EmptyString_OK()
        {
            _toSort = string.Empty;

            Assert.IsEmpty(_sorting.Sort(_toSort, true));
            Assert.IsEmpty(_sorting.Sort(_toSort, false));
        }

        [TestCase("A")]
        [TestCase("1")]
        [TestCase("4")]
        [TestCase("(")]
        public void StringWithLengthOf1symbol_OK(string toSort)
        {
            Assert.IsTrue(_sorting.Sort(toSort, true) == toSort);
            Assert.IsTrue(_sorting.Sort(toSort, false) == toSort);
        }

        [TestCase("AAAAAAAAAAAAAAA")]
        [TestCase("777777777777777777777777")]
        [TestCase("<<<<<<<<<<<<<<<")]
        public void StringWithTheSameSymbols_OK(string toSort)
        {
            Assert.IsTrue(_sorting.Sort(toSort, true) == toSort);
            Assert.IsTrue(_sorting.Sort(toSort, false) == toSort);
        }

        [Test]
        public void SortAsc_ArrayIsUnsorted_ArrayWasSorted()
        {
            // Arrange
            _toSort = GetRandomString(100);
            Console.WriteLine("String to sort: '{0}'", _toSort);

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
            _toSort = GetRandomString(100);
            Console.WriteLine("String to sort: '{0}'", _toSort);

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
