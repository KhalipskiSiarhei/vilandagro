using System;
using Microsoft.Owin.Hosting;

namespace Vilandagro.WebApi.Tests
{
    public static class WebApiStarter
    {
        public const string WebApiDefaultAddress = "http://localhost:9000";

        public static IDisposable RunWebApi(string baseAddress)
        {
            var api = WebApp.Start<WebApiOwinStartup>(baseAddress);
            return api;
        }

        public static IDisposable RunWebApi()
        {
            return RunWebApi(WebApiDefaultAddress);
        }
    }
}
