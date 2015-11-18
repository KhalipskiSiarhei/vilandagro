using NUnit.Framework;
using Vilandagro.Trainings.Algorithms;

namespace Vilandagro.Trainings.Tests.Algorithms
{
    [TestFixture]
    public class ArrayMergeSortTests : ArraySortTestFixture
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _sorting = new MergeSort();
        }
    }
}