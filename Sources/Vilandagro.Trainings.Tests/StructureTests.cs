using NUnit.Framework;

namespace Vilandagro.Trainings.Tests
{
    [TestFixture]
    public class StructureTests
    {
        private class TestClass
        {
            public int Value1 { get; set; }

            public string Value2 { get; set; }

            public object Value3 { get; set; }
        }

        private struct TestStructure
        {
            public int Value1 { get; set; }

            public string Value2 { get; set; }

            public TestClass Value3 { get; set; }
        }

        [Test]
        public void StructureTest()
        {
            var testClass = new TestClass() { Value1 = 10, Value2 = "20", Value3 = "30" };
            var testStructure1 = new TestStructure() { Value1 = 101, Value2 = "202", Value3 = testClass };
            var testStructure2 = testStructure1;

            // 1) - Initial Asserts
            Assert.IsTrue(testStructure1.Value1 == 101);
            Assert.IsTrue(testStructure1.Value2 == "202");
            Assert.IsTrue(testStructure1.Value3.Value1 == 10);
            Assert.IsTrue(testStructure1.Value3.Value2 == "20");
            Assert.IsTrue(testStructure1.Value3.Value3 == "30");

            Assert.IsTrue(testStructure2.Value1 == 101);
            Assert.IsTrue(testStructure2.Value2 == "202");
            Assert.IsTrue(testStructure2.Value3.Value1 == 10);
            Assert.IsTrue(testStructure2.Value3.Value2 == "20");
            Assert.IsTrue(testStructure2.Value3.Value3 == "30");

            // 2) - Change value types in the first structure
            testStructure1.Value1 = 102;
            testStructure1.Value2 = "203";

            // The another strucutre should contain initial values
            Assert.IsTrue(testStructure2.Value1 == 101);
            Assert.IsTrue(testStructure2.Value2 == "202");

            // 3) - Change reference type in the first structure
            testStructure1.Value3.Value1 = 11;
            testStructure1.Value3.Value2 = "21";
            testStructure1.Value3.Value3 = "31";

            Assert.IsTrue(testStructure2.Value3.Value1 == 11);
            Assert.IsTrue(testStructure2.Value3.Value2 == "21");
            Assert.IsTrue(testStructure2.Value3.Value3 == "31");

            testStructure1.Value3 = null;
            Assert.IsTrue(testStructure2.Value3 != null);
        }
    }
}
