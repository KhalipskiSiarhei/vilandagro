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
    public class StringInsertionSortTests : StringSortTestFixture
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _sorting = new InsertionSort();
        }
    }
}