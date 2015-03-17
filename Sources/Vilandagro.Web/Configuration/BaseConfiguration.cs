using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Configuration.DSL;

namespace Vilandagro.Web.Configuration
{
    public abstract class BaseConfiguration : Registry
    {
        protected BaseConfiguration()
        {
        }
    }
}