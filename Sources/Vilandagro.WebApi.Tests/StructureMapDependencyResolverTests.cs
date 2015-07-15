using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using NUnit.Framework;
using StructureMap;
using Vilandagro.Core;
using Vilandagro.Infrastructure;
using Vilandagro.WebApi.DependencyResolution;

namespace Vilandagro.WebApi.Tests
{
    [TestFixture]
    public class StructureMapDependencyResolverTests
    {
        private ILog _log = LogManager.GetLogger<StructureMapDependencyResolverTests>();

        private IContainer _container;

        private StructureMapDependencyResolver _resolver;

        [SetUp]
        public void SetUp()
        {
            _container = StructureMapContainer.GetContainer();
            _resolver = new StructureMapDependencyResolver(_container);
        }

        [TearDown]
        public void TearDown()
        {
            _resolver.Dispose();
        }

        [Test]
        public void ItIsTheSameControllerAsPassedToConstructor()
        {
            Assert.IsTrue(_resolver.Container == _container);
        }

        [Test]
        public void GetService_NoConfiguredType_NullReturned()
        {
            Assert.IsNull(_resolver.GetService(typeof(int)));
            Assert.IsNull(_resolver.GetService(typeof(StringBuilder)));
            Assert.IsNull(_resolver.GetService(typeof(IList)));
        }

        [Test]
        public void GetService_TypeWasConfigured_TypeReturned()
        {
            Assert.IsNotNull(_resolver.GetService(typeof(IRequestAware)));
        }

        [Test]
        public void GetServices_NoConfiguredType()
        {
            CollectionAssert.IsEmpty(_resolver.GetServices(typeof(int)));
            CollectionAssert.IsEmpty(_resolver.GetServices(typeof(StringBuilder)));
            CollectionAssert.IsEmpty(_resolver.GetServices(typeof(IList)));
        }

        [Test]
        public void GetServices_TypeWasConfigured_TypeReturned()
        {
            var services = _resolver.GetServices(typeof (IRequestAware)).ToList();
            CollectionAssert.IsNotEmpty(services);
            Assert.IsTrue(services.Single() is IRequestAware);
        }

        [Test]
        public void BeginScope_ContainersAreDifferent()
        {
            var dependencyScope = (StructureMapDependencyResolver)_resolver.BeginScope();

            Assert.IsTrue(dependencyScope.Container != _resolver.Container);
            _log.Info(dependencyScope.Container.WhatDoIHave());
            _log.Info(_resolver.Container.WhatDoIHave());
            Assert.IsTrue(dependencyScope.Container.WhatDoIHave().Replace("Profile is 'DEFAULT'\r\n", string.Empty) ==
                          _resolver.Container.WhatDoIHave());
        }

        [Test]
        public void BeginScope_ChangesInChildContainerDoNotAffectParentController()
        {
            var dependencyScope = (StructureMapDependencyResolver)_resolver.BeginScope();

            dependencyScope.Container.Configure(c => c.For<IRequestAware>().Use<ThreadRequestAware>().Singleton());
            Assert.IsTrue(dependencyScope.GetService(typeof(IRequestAware)) is ThreadRequestAware);
            Assert.IsTrue(_resolver.GetService(typeof(IRequestAware)) is WebRequestAware);
        }

        [Test]
        public void Dispose()
        {
            _resolver.Dispose();

            Assert.IsNull(_resolver.Container);
        }
    }
}