using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Configuration.DSL;
using Vilandagro.Web.Controllers.WebApi;

namespace Vilandagro.Web.Configuration
{
    public class WebApiConfiguration : BaseConfiguration
    {
        public WebApiConfiguration()
        {
            For<TestController>().Use<TestController>().Transient();
        }
    }
}