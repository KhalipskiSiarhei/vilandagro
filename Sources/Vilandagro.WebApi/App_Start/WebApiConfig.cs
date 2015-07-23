using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Common.Logging;
using Vilandagro.Core;
using Vilandagro.WebApi.DependencyResolution;
using Vilandagro.WebApi.Handlers;

namespace Vilandagro.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = StructureMapContainer.GetContainer();
            var requestAware = container.GetInstance<IRequestAware>();
            var log = container.GetInstance<ILog>();
            config.DependencyResolver = new StructureMapDependencyResolver(requestAware, container, log);
            GlobalConfiguration.Configuration.DependencyResolver = config.DependencyResolver;

            config.MessageHandlers.Add(
                (DelegatingHandler) config.DependencyResolver.GetService(typeof (LoggingMessageHandler)));
            config.MessageHandlers.Add(
                (DelegatingHandler) config.DependencyResolver.GetService(typeof (TransactionPerRequestMessageHandler)));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
