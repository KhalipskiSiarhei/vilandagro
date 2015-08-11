using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NUnit.Framework;
using Vilandagro.Core.Entities;

namespace Vilandagro.Infrastructure.EF.Tests.DifferentTypesOfQueries
{
    [TestFixture]
    public class EntitySQLViaObjectQuery : TestFixtureBase
    {
        private ObjectContext _context;

        protected override void SetUp()
        {
            base.SetUp();
            _context = ((IObjectContextAdapter)Repo.DbContext).ObjectContext;
        }

        [Test]
        public void GetQuery()
        {
            var sql = "SELECT c.Id, c.Name, c.Description, c.Image, c.Version FROM Categories AS c";
            var categoriesQuery = _context.CreateQuery<Category>(sql);
            var categories = categoriesQuery.Where(c => c.Version == 100).ToList();
        }

        [Test]
        public void Where()
        {
            var sql = "SELECT c.Id, c.Name, c.Description, c.Image, c.Version FROM Categories AS c";
            var categoriesQuery = _context.CreateQuery<Category>(sql);
            var categories = categoriesQuery.Where(c => c.Name == "123").ToList();
        }
    }
}