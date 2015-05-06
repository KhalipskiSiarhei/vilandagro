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
    public abstract class TestFixtureBase
    {
        protected static readonly ILog _log = LogManager.GetLogger<ILog>();

        private VilandagroDbContext _dbContext;
        private DbContextTransaction _transaction;
        private Repository _repo;

        protected Repository Repo
        {
            get { return _repo; }
        }

        [SetUp]
        public void SetUp()
        {
            _dbContext = new VilandagroDbContext();
            _transaction = _dbContext.Database.BeginTransaction();

            _repo = new Repository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}