using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vilandagro.Trainings.Big;

namespace Vilandagro.Trainings.Tests.Big
{
    [TestFixture]
    public class BigTextFileHelperTests
    {
        [TestCase(2)]
        public void GenerateFile(int sizeGb)
        {
            var start = DateTime.Now;
            BigTextFileHelper.GenerateFile(sizeGb);
            var end = DateTime.Now;

            Console.WriteLine("Generation time: {0}", (end - start).TotalSeconds);
        }

        //[Test]
        //public void MergeFiles()
        //{
        //    var start = DateTime.Now;
        //    BigTextFileHelper.MergeFiles("0c2e9640-5ca8-40c6-9566-ddf7bffe8c08.txt", "0c2e9640-5ca8-40c6-9566-ddf7bffe8c09.txt");
        //    var end = DateTime.Now;

        //    Console.WriteLine("Merging time: {0}", (end - start).TotalSeconds);
        //}

        [Test]
        public void SplitFileInto2Files()
        {
            var start = DateTime.Now;
            var result = BigTextFileHelper.SplitFileInto2Files("0c2e9640-5ca8-40c6-9566-ddf7bffe8c08.txt");
            var end = DateTime.Now;

            Console.WriteLine("Splitted time: {0}", (end - start).TotalSeconds);
        }
    }
}