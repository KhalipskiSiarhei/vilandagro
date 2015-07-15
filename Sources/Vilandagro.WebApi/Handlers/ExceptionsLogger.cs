using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;
using Common.Logging;
using Vilandagro.Core.Exceptions;

namespace Vilandagro.WebApi.Handlers
{
    public class ExceptionsLogger : IExceptionLogger
    {
        private readonly ILog _log;

        public static readonly Task CompletedTask = Task.FromResult(true);

        public ExceptionsLogger(ILog log)
        {
            _log = log;
        }

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            if (context.Exception is BusinessException)
            {
                _log.Debug(string.Format("Business exception {0} {1} | CatchBlock={2} | Exception={3}",
                    context.Request.Method,
                    context.Request.RequestUri,
                    GetCatchBlockString(context.CatchBlock), context.Exception));
            }
            else
            {
                _log.Error(string.Format("Unhandled exception {0} {1} | CatchBlock={2} | Exception={3}",
                    context.Request.Method,
                    context.Request.RequestUri,
                    GetCatchBlockString(context.CatchBlock), context.Exception));
            }
            return CompletedTask;
        }

        private string GetCatchBlockString(ExceptionContextCatchBlock catchBlock)
        {
            return string.Format("{{ Name:{0}, IsTopLevel:{1}, CallsHandler:{2} }}", catchBlock.Name, catchBlock.IsTopLevel,
                catchBlock.CallsHandler);
        }
    }
}