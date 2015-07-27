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
        public const string TransactionPattern = "TransactionPattern";

        private const string FailedResultKey = "FailedResult";

        protected readonly ILog _log;

        protected readonly IRequestAware _requestAware;

        protected readonly DbContextManager _dbContextManager;

        public TransactionPerRequestMessageHandler(ILog log, IRequestAware requestAware, DbContextManager dbContextManager)
        {
            _log = log;
            _requestAware = requestAware;
            _dbContextManager = dbContextManager;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var isContextCreated = false;
            var isTransactionCreated = false;

            // Batch request
            if (IsBatchRequest(request))
            {
                var transactionPattern = GetBatchTransactionPattern(request);

                _log.DebugFormat("Starting batch request with the transaction={0}", transactionPattern);
                if (transactionPattern == TransactionsPattern.PerBatch)
                {
                    if (!_dbContextManager.IsCreatedDbContext())
                    {
                        _dbContextManager.CreateDbContext();
                        isContextCreated = true;
                    }

                    if (!_dbContextManager.IsOpenedTransaction())
                    {
                        _dbContextManager.BeginTransaction();
                        isTransactionCreated = true;
                        _log.Debug("New transaction has been initiated for the batch request");
                    }
                    else
                    {
                        _log.Debug("Already existed transaction has been initiated for the batch request");
                    }

                    var response = await base.SendAsync(request, cancellationToken);
                    SpecifyFailedResult(response.IsSuccessStatusCode);

                    if (isTransactionCreated)
                    {
                        if (response.IsSuccessStatusCode && !IsFailedResult())
                        {
                            _dbContextManager.CommitTransaction();
                            _log.Debug("Transaction has been committed for the batch request");
                        }
                        else
                        {
                            _dbContextManager.RollbackTransaction();
                            _log.Debug("Transaction has been rollbacked for the batch request");
                        }
                    }
                    else
                    {
                        _log.Debug("Transaction has not been finished because it is managed out of the current batch request");
                    }

                    if (isContextCreated)
                    {
                        _dbContextManager.ClearDbContext();
                    }
                    return response;
                }
            }
            // Not batch request
            else
            {
                if (!_dbContextManager.IsCreatedDbContext())
                {
                    _dbContextManager.CreateDbContext();
                    isContextCreated = true;
                }

                if (!_dbContextManager.IsOpenedTransaction())
                {
                    _dbContextManager.BeginTransaction();
                    isTransactionCreated = true;
                    _log.Debug("New transaction has been initiated for the request");
                }
                else
                {
                    _log.Debug("Already existed transaction has been initiated for the request");
                }

                var response = await base.SendAsync(request, cancellationToken);
                SpecifyFailedResult(response.IsSuccessStatusCode);

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
                else
                {
                    _log.Debug("Transaction has not been finished because it is managed out of the current request");
                }

                if (isContextCreated)
                {
                    _dbContextManager.ClearDbContext();
                }
                return response;
            }

            return await base.SendAsync(request, cancellationToken);
        }

        protected bool SpecifyFailedResult(bool IsSuccessStatusCode)
        {
            if (!IsSuccessStatusCode)
            {
                _requestAware[FailedResultKey] = true;
            }

            return IsFailedResult();
        }

        protected bool IsFailedResult()
        {
            if (_requestAware[FailedResultKey] != null)
            {
                return (bool)_requestAware[FailedResultKey];
            }

            return false;
        }

        protected bool IsBatchRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsoluteUri.Contains("api/batch");
        }

        private TransactionsPattern GetBatchTransactionPattern(HttpRequestMessage request)
        {
            if (request.Headers.Contains(TransactionPattern))
            {
                return
                    (TransactionsPattern)
                        Enum.Parse(typeof(TransactionsPattern),
                            request.Headers.Single(s => s.Key == TransactionPattern).Value.Single());
            }

            return WebApi.TransactionsPattern.PerBatch;
        }
    }
}