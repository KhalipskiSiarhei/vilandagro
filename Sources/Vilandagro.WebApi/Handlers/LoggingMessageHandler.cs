using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Common.Logging;

namespace Vilandagro.WebApi.Handlers
{
    public class LoggingMessageHandler : DelegatingHandler
    {
        private readonly ILog _log;

        public LoggingMessageHandler(ILog log)
        {
            _log = log;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _log.DebugFormat("Starting {0} {1}", request.Method, request.RequestUri);

            var response = await base.SendAsync(request, cancellationToken);

            _log.DebugFormat("Finished {0} {1} | StatusCode={2}", request.Method, request.RequestUri, response.StatusCode);
            return response;
        }
    }
}