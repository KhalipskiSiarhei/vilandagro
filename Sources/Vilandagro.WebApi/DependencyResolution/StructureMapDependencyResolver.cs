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
        private const string ChildScopeKey = "8CA0DB2B-21BB-4D24-B8D6-C8C7F9833806";

        private readonly IRequestAware _requestAware;

        public StructureMapDependencyResolver(IRequestAware requestAware, IContainer container, ILog log)
            : base(container, log)
        {
            _requestAware = requestAware;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _requestAware[ChildScopeKey] = null;
            }

            base.Dispose(disposing);
        }

        public IDependencyScope BeginScope()
        {
            if (!(_requestAware[ChildScopeKey] is IDependencyScope))
            {
                _requestAware[ChildScopeKey] = new StructureMapDependencyResolver(_requestAware, Container.GetNestedContainer(), _log);
                _log.Debug("DependencyScope has been created");
            }
            return (IDependencyScope) _requestAware[ChildScopeKey];
        }
    }
}