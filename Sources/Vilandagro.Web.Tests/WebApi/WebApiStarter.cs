using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace Vilandagro.Web.Tests.WebApi
{
    public static class WebApiStarter
    {
        public const string WebApiDefaultAddress = "http://localhost:9000/";

        public static IDisposable RunWebApi(string baseAddress)
        {
            var api = WebApp.Start<WebApiStartup>(baseAddress);
            return api;
        }

        public static IDisposable RunWebApi()
        {
            return RunWebApi(WebApiDefaultAddress);
        }
    }
}
