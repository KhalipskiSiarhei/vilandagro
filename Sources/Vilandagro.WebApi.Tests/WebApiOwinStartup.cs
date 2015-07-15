using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Vilandagro.WebApi.Tests.WebApiOwinStartup))]

namespace Vilandagro.WebApi.Tests
{
    public class WebApiOwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}