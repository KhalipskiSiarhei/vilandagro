using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vilandagro.WebApi.Tests
{
    public class WebApiControllerTestFixtureBase
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

        protected async Task<HttpResponseMessage> Get(string relativePath, params object[] args)
        {
            using (var httpClient = new HttpClient())
            {
                var url = GetRequesturl(
                    WebApiStarter.WebApiDefaultAddress,
                    relativePath,
                    args != null ? string.Join("/", args) : string.Empty);
                return await httpClient.GetAsync(url);
            }
        }

        private string GetRequesturl(string baseAddress, string relativePath, string args)
        {
            return string.Concat(baseAddress, relativePath, args);
        }
    }
}