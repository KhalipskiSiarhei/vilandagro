using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;
using Common.Logging;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using Vilandagro.Core;
using Vilandagro.Infrastructure;
using Vilandagro.WebApi.Controllers;
using Vilandagro.WebApi.Handlers;

namespace Vilandagro.WebApi.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });

            For<ILog>().Use(LogManager.GetLogger("Vilandagro.WebApi")).Singleton();
            For<IRequestAware>().Use<WebRequestAware>().Singleton();

            For<IExceptionLogger>().Use<ExceptionsLogger>().Singleton();
            For<IExceptionHandler>().Use<ExceptionsHandler>().Singleton();
            For<LoggingMessageHandler>().Use<LoggingMessageHandler>().Singleton();
            For<TransactionPerRequestMessageHandler>().Use<TransactionPerRequestMessageHandler>().Singleton();
        }

        #endregion
    }
}