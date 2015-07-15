using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;

namespace Vilandagro.WebApi.DependencyResolution
{
    public class StructureMapContainer
    {
        private StructureMapContainer()
        {
        }

        public static IContainer GetContainer()
        {
            return new Container(c => c.AddRegistry<DefaultRegistry>());
        }
    }
}