using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Configuration.DSL;
using Vilandagro.Web.Controllers.MVC;

namespace Vilandagro.Web.Configuration
{
    public class MvcConfiguration : BaseConfiguration
    {
        public MvcConfiguration()
        {
            For<HomeController>().Use<HomeController>().Transient();
        }
    }
}