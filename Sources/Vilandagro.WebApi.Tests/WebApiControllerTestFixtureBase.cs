using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Common.Logging;
using NUnit.Framework;
using Vilandagro.Infrastructure.EF;

namespace Vilandagro.WebApi.Tests
{
    public class WebApiControllerTestFixtureBase
    {
        private ILog _log = LogManager.GetLogger<WebApiControllerTestFixtureBase>();

        private IDisposable _api;

        private DbContextManager _dbContextManager;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _api = WebApiStarter.RunWebApi();
        }

        [SetUp]
        public virtual void SetUp()
        {
            _dbContextManager = (DbContextManager)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(DbContextManager));

            _log.DebugFormat("New DbContext has been created for the test");
            _dbContextManager.CreateDbContext();

            _log.DebugFormat("Begin new transaction for the test");
            _dbContextManager.BeginTransaction();
        }

        [TearDown]
        public virtual void TearDown()
        {
            _dbContextManager.RollbackTransaction();
            _log.DebugFormat("Transaction for the test has been rollbacked");

            _dbContextManager.ClearDbContext();
            _log.DebugFormat("DbContext for the test has been cleared");
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