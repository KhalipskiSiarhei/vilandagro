using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Common.Logging;
using Vilandagro.Core;
using Vilandagro.Infrastructure.EF;

namespace Vilandagro.WebApi.Handlers
{
    public class TransactionPerRequestMessageHandler : DelegatingHandler
    {
        private readonly ILog _log;

        private readonly DbContextManager _dbContextManager;

        public TransactionPerRequestMessageHandler(ILog log, DbContextManager dbContextManager)
        {
            _log = log;
            _dbContextManager = dbContextManager;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var isContextCreated = false;
            var isTransactionCreated = false;

            if (!_dbContextManager.IsCreatedDbContext())
            {
                _dbContextManager.CreateDbContext();
                isContextCreated = true;
            }

            if (!_dbContextManager.IsOpenedTransaction())
            {
                _dbContextManager.BeginTransaction();
                isTransactionCreated = true;
                _log.Debug("New or already created transaction has been initiated for the request");
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (isTransactionCreated)
            {
                if (response.IsSuccessStatusCode)
                {
                    _dbContextManager.CommitTransaction();
                    _log.Debug("Transaction has been committed for the request");
                }
                else
                {
                    _dbContextManager.RollbackTransaction();
                    _log.Debug("Transaction has been rollbacked for the request");
                }
            }

            if (isContextCreated)
            {
                _dbContextManager.ClearDbContext();
            }
            return response;
        }
    }
}