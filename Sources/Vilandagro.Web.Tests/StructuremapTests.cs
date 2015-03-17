using NUnit.Framework;
using StructureMap;
using Vilandagro.Web.App_Start;

namespace Vilandagro.Web.Tests
{
    [TestFixture]
    public class StructuremapTests : TestFixtureBase
    {
        [Test]
        public void MvcAssertConfigurationIsValid()
        {
            var container = (Container)StructuremapMvc.GetConfiguredContainer();

            container.AssertConfigurationIsValid();
            Log.Info(container.WhatDoIHave());
        }

        [Test]
        public void WebApiAssertConfigurationIsValid()
        {
            var container = StructuremapWebApi.GetConfiguredContainer();

            container.AssertConfigurationIsValid();
            Log.Info(container.WhatDoIHave());
        }
    }
}
