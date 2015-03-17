using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vilandagro.Web.Tests.WebApi
{
    [TestFixture]
    public class TestControllerTests : WebApiControllerTestFixtureBase
    {
        [Test]
        public void GetAll()
        {
            var result = Get<string[]>("api/test/");

            Assert.IsTrue(result.Length == 2);
            Assert.IsTrue(result.Contains("value1"));
            Assert.IsTrue(result.Contains("value2"));
        }

        [TestCase("1")]
        [TestCase("2")]
        public void GetById(string id)
        {
            var result = Get<string>("api/test/", id);
            Assert.IsTrue(result == "value");
        }
    }
}
