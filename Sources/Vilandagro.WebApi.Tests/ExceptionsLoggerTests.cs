using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using NSubstitute;
using NUnit.Framework;
using Vilandagro.WebApi.Handlers;
using ExceptionContext = System.Web.Http.ExceptionHandling.ExceptionContext;
using ExceptionContextCatchBlock = System.Web.Http.ExceptionHandling.ExceptionContextCatchBlock;
using ExceptionLoggerContext = System.Web.Http.ExceptionHandling.ExceptionLoggerContext;

namespace Vilandagro.WebApi.Tests
{
    [TestFixture]
    public class ExceptionsLoggerTests
    {
        private ILog _log;
        private ExceptionsLogger _logger;

        [SetUp]
        public void SetUp()
        {
            _log = Substitute.For<ILog>();
            _logger = new ExceptionsLogger(_log);
        }

        [Test]
        public void LogAsync()
        {
            var task = _logger.LogAsync(
                new ExceptionLoggerContext(new ExceptionContext(new Exception(),
                    new ExceptionContextCatchBlock("CatchBlockName", true, false),
                    new HttpRequestMessage(HttpMethod.Get, new Uri("http://tut.by")))), new CancellationToken(false));

            Assert.IsTrue(task == ExceptionsLogger.CompletedTask);
            _log.Received(1).Error(Arg.Any<string>());
        }
    }
}