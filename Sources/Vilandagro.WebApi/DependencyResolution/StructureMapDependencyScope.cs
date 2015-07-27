using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Common.Logging;
using StructureMap;

namespace Vilandagro.WebApi.DependencyResolution
{
    public class StructureMapDependencyScope : IDependencyScope
    {
        private IContainer _container;

        protected ILog _log;

        public StructureMapDependencyScope(IContainer container, ILog log)
        {
            _container = container;
            _log = log;
        }

        protected internal IContainer Container
        {
            get { return _container; }
            set { _container = value; }
        }

        public object GetService(Type serviceType)
        {
            return Container.TryGetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.GetAllInstances(serviceType).Cast<object>();
        }

        #region IDisposable
        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~StructureMapDependencyScope() 
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) 
            {
                if (Container != null)
                {
                    Container.Dispose();
                    Container = null;
                }
            }
        }
        #endregion
    }
}