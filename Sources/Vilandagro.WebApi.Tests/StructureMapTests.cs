using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using NUnit.Framework;
using StructureMap;
using Vilandagro.WebApi.DependencyResolution;
using Vilandagro.WebApi.Handlers;

namespace Vilandagro.WebApi.Tests
{
    [TestFixture]
    public class StructureMapTests
    {
        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            _container = StructureMapContainer.GetContainer();
        }

        [Test]
        public void AssertConfigurationIsValid()
        {
            Console.WriteLine(_container.WhatDoIHave());
            _container.AssertConfigurationIsValid();
        }

        [Test]
        public void ExceptionsLoggerShouldBeConfigured()
        {
            Assert.IsTrue(_container.GetInstance<IExceptionLogger>() is ExceptionsLogger);
        }

        [Test]
        public void ExceptionsHandlerShouldBeConfigured()
        {
            Assert.IsTrue(_container.GetInstance<IExceptionHandler>() is ExceptionsHandler);
        }
    }
}