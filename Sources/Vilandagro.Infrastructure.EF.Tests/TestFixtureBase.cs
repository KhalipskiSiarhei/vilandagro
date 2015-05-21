using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using NUnit.Framework;

namespace Vilandagro.Infrastructure.EF.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public abstract class TestFixtureBase
    {
        protected static readonly ILog Log = LogManager.GetLogger<ILog>();

        protected VilandagroDbContext _dbContext;
        private DbContextTransaction _transaction;
        private Repository _repo;

        protected Repository Repo
        {
            get { return _repo; }
        }

        [SetUp]
        protected virtual void SetUp()
        {
            _dbContext = new VilandagroDbContext();
            _transaction = _dbContext.Database.BeginTransaction();

            _repo = new Repository(_dbContext);
        }

        [TearDown]
        protected virtual void TearDown()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }

            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }
    }
}