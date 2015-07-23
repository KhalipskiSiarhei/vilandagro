using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;
using Common.Logging;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Pipeline;
using Vilandagro.Core;
using Vilandagro.Infrastructure;
using Vilandagro.Infrastructure.EF;
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
            // IRequestAware is singleton because in the Web environment it is used HttpContext.Items object to store values
            // For the test environment Container is recreated per every test
            For<IRequestAware>().Use<WebRequestAware>().Singleton();
            For<IRepository>().Use<Repository>().Singleton().Ctor<IRequestAware>();
            For<DbContextManager>().Use<DbContextManager>().Singleton().Ctor<IRequestAware>();
            
            For<IExceptionLogger>().Use<ExceptionsLogger>().Singleton();
            For<IExceptionHandler>().Use<ExceptionsHandler>().Singleton();
            For<LoggingMessageHandler>().Use<LoggingMessageHandler>().Singleton();
            For<TransactionPerRequestMessageHandler>().Use<TransactionPerRequestMessageHandler>().Singleton();
        }

        #endregion
    }
}