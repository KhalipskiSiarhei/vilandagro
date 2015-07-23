using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vilandagro.Infrastructure.Tests
{
    [TestFixture]
    public class CustomRequestAwareTests
    {
        private CustomRequestAware _requestAware;

        [SetUp]
        public void SetUp()
        {
            _requestAware = new CustomRequestAware();
        }

        [Test]
        public void GetValueViaAccessor_NoValue_NullReturned()
        {
            Assert.IsNull(_requestAware[Guid.NewGuid().ToString()]);
        }

        [Test]
        public void GetValueViaAccessor_ThereIsValue_ValueReturned()
        {
            var key = Guid.NewGuid().ToString();

            _requestAware[key] = "Hello World!";
            Assert.IsTrue((string)_requestAware[key] == "Hello World!");
        }

        [Test]
        public void GetValueViaAccessor_ThereIsResetValue_ValueReturned()
        {
            var key = Guid.NewGuid().ToString();

            _requestAware[key] = "Hello World!";
            _requestAware[key] = "Minsk";
            Assert.IsTrue((string)_requestAware[key] == "Minsk");
        }

        [Test]
        public void GetValueViaAccessor_ValueIsNullAfterCleanup_NullReturned()
        {
            var key = Guid.NewGuid().ToString();

            _requestAware[key] = "Hello World!";
            _requestAware[key] = null;
            Assert.IsNull(_requestAware[key]);
        }

        [Test]
        public void GetValue_NoValue_NullReturned()
        {
            Assert.IsNull(_requestAware.GetValue<string>(Guid.NewGuid().ToString()));
        }

        [Test]
        public void GetValue_ThereIsValue_ValueReturned()
        {
            var key = Guid.NewGuid().ToString();

            _requestAware[key] = "Hello World!";
            Assert.IsTrue(_requestAware.GetValue<string>(key) == "Hello World!");
        }

        [Test]
        public void GetValue_ThereIsResetValue_ValueReturned()
        {
            var key = Guid.NewGuid().ToString();

            _requestAware[key] = "Hello World!";
            _requestAware[key] = "Minsk";
            Assert.IsTrue(_requestAware.GetValue<string>(key) == "Minsk");
        }

        [Test]
        public void GetValue_ValueIsNullAfterCleanup_NullReturned()
        {
            var key = Guid.NewGuid().ToString();

            _requestAware[key] = "Hello World!";
            _requestAware[key] = null;
            Assert.IsNull(_requestAware.GetValue<string>(key));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetValueWithExceptionIfNull_NoValue_ExceptionThrow()
        {
            _requestAware.GetValue<string>(Guid.NewGuid().ToString(), true);
        }

        [Test]
        public void GetValueWithExceptionIfNull_ThereIsValue_ValueReturned()
        {
            var key = Guid.NewGuid().ToString();

            _requestAware[key] = "Hello World!";
            Assert.IsTrue(_requestAware.GetValue<string>(key, true) == "Hello World!");
        }

        [Test]
        public void GetValueWithExceptionIfNull_ThereIsResetValue_ValueReturned()
        {
            var key = Guid.NewGuid().ToString();

            _requestAware[key] = "Hello World!";
            _requestAware[key] = "Minsk";
            Assert.IsTrue(_requestAware.GetValue<string>(key, true) == "Minsk");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetValueWithExceptionIfNull_ValueIsNullAfterCleanup_ExceptionThrow()
        {
            var key = Guid.NewGuid().ToString();

            _requestAware[key] = "Hello World!";
            _requestAware[key] = null;
            _requestAware.GetValue<string>(key, true);
        }
    }
}
