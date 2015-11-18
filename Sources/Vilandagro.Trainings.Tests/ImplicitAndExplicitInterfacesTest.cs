using System;
using NUnit.Framework;

namespace Vilandagro.Trainings.Tests
{
    public interface ITest1
    {
        void Test1();

        void Test2();
    }

    public interface ITest2
    {
        void Test1();

        void Test2();
    }

    public class TestClass1 : ITest1
    {
        public virtual void Test1()
        {
            Console.WriteLine("TestClass1.Test1.Test1");
        }

        public virtual void Test2()
        {
            Console.WriteLine("TestClass1.Ttest1.Test2");
        }
    }

    public class TestClass2 : TestClass1, ITest2
    {
        public new virtual void Test2()
        {
            Console.WriteLine("Override TestClass1.Ttest1.Test2");
        }

        void ITest2.Test1()
        {
            Console.WriteLine("TestClass1.Test2.Test1");
        }

        void ITest2.Test2()
        {
            Console.WriteLine("TestClass1.Ttest2.Test2");
        }
    }

    [TestFixture]
    public class ImplicitAndExplicitInterfacesTest
    {
        [Test]
        public void TestClass2()
        {
            var testClass2 = new TestClass2();
            var itest1 = (ITest1)testClass2;
            var itest2 = (ITest2)testClass2;

            itest1.Test1();
            itest1.Test2();

            itest2.Test1();
            itest2.Test2();
        }
    }
}