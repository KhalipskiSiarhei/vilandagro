using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Common.Logging;
using StructureMap;
using Vilandagro.Core;

namespace Vilandagro.WebApi.DependencyResolution
{
    public class StructureMapDependencyResolver : StructureMapDependencyScope, IDependencyResolver
    {
        public StructureMapDependencyResolver(IContainer container, ILog log)
            : base(container, log)
        {
        }

        protected override void Dispose(bool disposing)
        {
            _log.Debug("DependencyScope has been disposed...");
            base.Dispose(disposing);
        }

        public IDependencyScope BeginScope()
        {
            var scope =  new StructureMapDependencyResolver(Container.GetNestedContainer(), _log);

            _log.DebugFormat("DependencyScope has been created!");
            return scope;
        }
    }
}