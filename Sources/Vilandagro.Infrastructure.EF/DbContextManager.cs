using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vilandagro.Core;

namespace Vilandagro.Infrastructure.EF
{
    public class DbContextManager
    {
        public const string DbContextKey = "2AB91918-1C2D-4E67-8B48-8B690EEA0E21";

        private readonly IRequestAware _requestAware;

        public DbContextManager(IRequestAware requestAware)
        {
            _requestAware = requestAware;
        }

        private DbContext CurrentDbContext
        {
            get
            {
                var dbContext = (DbContext) _requestAware[DbContextKey];
                return dbContext;
            }
            set { _requestAware[DbContextKey] = value; }
        }

        public bool IsCreatedDbContext()
        {
            return CurrentDbContext != null;
        }

        public void CreateDbContext()
        {
            if (CurrentDbContext != null)
            {
                throw new InvalidOperationException();
            }

            CurrentDbContext = new VilandagroDbContext();
        }

        public void ClearDbContext()
        {
            if (CurrentDbContext != null)
            {
                CurrentDbContext.Dispose();
                CurrentDbContext = null;
            }
        }

        public bool IsOpenedTransaction()
        {
            if (CurrentDbContext == null || CurrentDbContext.Database == null)
            {
                throw new InvalidOperationException();
            }

            return CurrentDbContext.Database.CurrentTransaction != null;
        }

        public void UseTransaction(DbTransaction transaction)
        {
            if (CurrentDbContext == null || CurrentDbContext.Database == null || CurrentDbContext.Database.CurrentTransaction != null)
            {
                throw new InvalidOperationException();
            }

            CurrentDbContext.Database.UseTransaction(transaction);
        }

        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (CurrentDbContext == null || CurrentDbContext.Database == null || CurrentDbContext.Database.CurrentTransaction != null)
            {
                throw new InvalidOperationException();
            }

            CurrentDbContext.Database.BeginTransaction(isolationLevel);
        }

        public void CommitTransaction()
        {
            if (CurrentDbContext == null || CurrentDbContext.Database == null || CurrentDbContext.Database.CurrentTransaction == null)
            {
                throw new InvalidOperationException();
            }

            CurrentDbContext.Database.CurrentTransaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (CurrentDbContext == null || CurrentDbContext.Database == null || CurrentDbContext.Database.CurrentTransaction == null)
            {
                throw new InvalidOperationException();
            }

            CurrentDbContext.Database.CurrentTransaction.Rollback();
        }
    }
}