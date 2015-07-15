using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using StructureMap.Graph;
using StructureMap.Pipeline;
using StructureMap.TypeRules;
using StructureMap.Configuration.DSL;

namespace Vilandagro.WebApi.DependencyResolution
{
    public class ControllerConvention : IRegistrationConvention
    {
        #region Public Methods and Operators

        public void Process(Type type, Registry registry)
        {
            if (type.CanBeCastTo<ApiController>() && !type.IsAbstract)
            {
                registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
            }
        }

        #endregion
    }
}