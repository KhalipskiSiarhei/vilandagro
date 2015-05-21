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
            _context = ((IObjectContextAdapter)_dbContext).ObjectContext;
        }

        protected override void TearDown()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
            base.TearDown();
        }

        [Test]
        public void GetQuery()
        {
            var sql = "SELECT c.Id, c.Name, c.Description, c.Image FROM Categories AS c";
            var categoriesQuery = _context.CreateQuery<Category>(sql);
            var categories = categoriesQuery.ToList();
        }

        [Test]
        public void Where()
        {
            var sql = "SELECT c.Id, c.Name, c.Description, c.Image FROM Categories AS c";
            var categoriesQuery = _context.CreateQuery<Category>(sql);
            var categories = categoriesQuery.Where(c => c.Name == "123").ToList();
        }
    }
}