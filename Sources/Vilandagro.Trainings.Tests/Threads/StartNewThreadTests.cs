using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vilandagro.Trainings.Tests.Threads
{
    [TestFixture]
    public class StartNewThreadTests
    {
        private class TestThread
        {
            private bool _done = false;

            public static int CoundOfGoRun = 0;

            public void Go()
            {
                if (!_done)
                {
                    CoundOfGoRun++;
                    Console.WriteLine(CoundOfGoRun);
                    _done = true;
                }
            }
        }

        [Test]
        public void StartThread()
        {
            var testThread = new TestThread();

            new Thread(testThread.Go).Start();
            testThread.Go();

            Assert.IsTrue(TestThread.CoundOfGoRun == 2);
        }
    }
}