using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vilandagro.Trainings.Big;

namespace Vilandagro.Trainings.Tests.Big
{
    [TestFixture]
    public class BigFileSortingTests
    {
        private BigFileSorting _bigFileSorting;

        [SetUp]
        public void SetUp()
        {
            _bigFileSorting = new BigFileSorting(BigTextFileHelper.OneGbLong/20, 0.1);
        }

        [Test]
        public void SortSync()
        {
            var file = BigTextFileHelper.GenerateFile(0.5);
            _bigFileSorting.SortSync(file);
        }
    }
}