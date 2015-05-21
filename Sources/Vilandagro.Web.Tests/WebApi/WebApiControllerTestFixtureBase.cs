using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vilandagro.Web.Tests.WebApi
{
    public class WebApiControllerTestFixtureBase : TestFixtureBase
    {
        private IDisposable _api;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _api = WebApiStarter.RunWebApi();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            if (_api != null)
            {
                _api.Dispose();
                _api = null;
            }
        }

        protected TResult Get<TResult>(string relativePath, params object[] args)
        {
            using (var httpClient = new HttpClient())
            {
                var url = GetRequesturl(
                    WebApiStarter.WebApiDefaultAddress,
                    relativePath,
                    args != null ? string.Join("/", args) : string.Empty);
                var response = httpClient.GetAsync(url).Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                return response.Content.ReadAsAsync<TResult>().Result;
            }
        }

        private string GetRequesturl(string baseAddress, string relativePath, string args)
        {
            return string.Concat(baseAddress, relativePath, args);
        }
    }
}