using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using Vilandagro.Web.App_Start;

[assembly: OwinStartup(typeof(Vilandagro.Web.Tests.WebApiStartup))]

namespace Vilandagro.Web.Tests
{
    public class WebApiStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            StructuremapWebApi.Start();
            WebApiConfig.Register(config);

            app.UseWebApi(config);
        }
    }
}
