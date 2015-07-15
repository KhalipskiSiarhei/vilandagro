using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using StructureMap;

namespace Vilandagro.WebApi.DependencyResolution
{
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        private IContainer _container;

        public StructureMapDependencyResolver(IContainer container)
        {
            _container = container;
        }

        protected internal IContainer Container
        {
            get { return _container; }
            set { _container = value; }
        }

        public void Dispose()
        {
            if (Container != null)
            {
                Container.Dispose();
                Container = null;
            }
        }

        public object GetService(Type serviceType)
        {
            return Container.TryGetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.GetAllInstances(serviceType).Cast<object>();
        }

        public IDependencyScope BeginScope()
        {
            return new StructureMapDependencyResolver(Container.CreateChildContainer());
        }
    }
}