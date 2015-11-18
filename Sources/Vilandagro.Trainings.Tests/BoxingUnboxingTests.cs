using System;
using NUnit.Framework;

namespace Vilandagro.Trainings.Tests
{
    [TestFixture]
    public class BoxingUnboxingWithInterfacesTests
    {
        internal interface IChangeBoxedPoint
        {
            void Change(Int32 x, Int32 y);
        }

        internal struct Point1 : IChangeBoxedPoint
        {
            private Int32 m_x, m_y;

            public Point1(Int32 x, Int32 y)
            {
                m_x = x;
                m_y = y;
            }

            public void Change(Int32 x, Int32 y)
            {
                m_x = x;
                m_y = y;
            }

            public override String ToString()
            {
                return String.Format("({0}, {1})", m_x, m_y);
            }
        }

        internal struct Point2
        {
            private Int32 m_x, m_y;

            public Point2(Int32 x, Int32 y)
            {
                m_x = x;
                m_y = y;
            }
            public void Change(Int32 x, Int32 y)
            {
                m_x = x; m_y = y;
            }
            public override String ToString()
            {
                return String.Format("({0}, {1})", m_x, m_y);
            }
        } 

        [Test]
        public void TestWithStructureAndInterface()
        {
            var p = new Point1(1, 1);

            p.Change(2, 2);
            Assert.IsTrue(p.ToString() == "(2, 2)");

            Object o = p;
            Assert.IsTrue(o.ToString() == "(2, 2)");

            ((Point1)o).Change(3, 3);
            Assert.IsTrue(o.ToString() == "(2, 2)");

            ((IChangeBoxedPoint)p).Change(4, 4);
            Assert.IsTrue(p.ToString() == "(2, 2)");

            // Упакованный объект изменяется и выводится
            ((IChangeBoxedPoint)o).Change(5, 5);
            Assert.IsTrue(o.ToString() == "(5, 5)");
        }

        [Test]
        public void TestWithStructure()
        {
            var p = new Point1(1, 1);

            p.Change(2, 2);
            Assert.IsTrue(p.ToString() == "(2, 2)");

            Object o = p;
            Assert.IsTrue(o.ToString() == "(2, 2)");

            ((Point1) o).Change(3, 3);
            Console.WriteLine(o);
            Assert.IsTrue(o.ToString() == "(2, 2)");
        }
    }
}