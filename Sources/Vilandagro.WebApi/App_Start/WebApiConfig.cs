﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Batch;
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
            ConfigureServer(config, GlobalConfiguration.DefaultServer);
        }

        public static HttpServer RegisterOwin(HttpConfiguration config)
        {
            return ConfigureServer(config);
        }

        private static HttpServer ConfigureServer(HttpConfiguration config, HttpServer server = null)
        {
            server = server ?? new HttpServer(config);

            // Web API configuration and services
            var container = StructureMapContainer.GetContainer();
            var log = container.GetInstance<ILog>();
            config.DependencyResolver = new StructureMapDependencyResolver(container, log);
            GlobalConfiguration.Configuration.DependencyResolver = config.DependencyResolver;

            config.MessageHandlers.Add(
                (DelegatingHandler)config.DependencyResolver.GetService(typeof(LoggingMessageHandler)));
            config.MessageHandlers.Add(
                (DelegatingHandler)config.DependencyResolver.GetService(typeof(TransactionPerRequestMessageHandler)));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpBatchRoute(
                routeName: "batch",
                routeTemplate: "api/batch",
                batchHandler: new DefaultHttpBatchHandler(server));
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            return server;
        }
    }
}
