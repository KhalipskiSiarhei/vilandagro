using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Vilandagro.Core.Exceptions;

namespace Vilandagro.WebApi.Handlers
{
    public class ExceptionsHandler : IExceptionHandler
    {
        public static Task CompletedTask = Task.FromResult(true);

        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            var controller = (ApiController) context.ExceptionContext.ControllerContext.Controller;
            HttpResponseMessage responce = null;

            if (context.Exception is NotFoundException)
            {
                responce = context.Request.CreateErrorResponse(HttpStatusCode.NotFound, context.Exception.Message);
            }
            else if (context.Exception is ModelStateException)
            {
                var typedException = (ModelStateException)context.Exception;
                responce = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, typedException.ModelState);
            }
            else if (context.Exception is BusinessException)
            {
                responce = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, context.Exception.Message);
            }
            else
            {
                context.Result = new InternalServerErrorResult(context.Request);
            }

            if (responce != null)
            {
                context.Result = GetActionResultByResponceMessage(controller, responce);
            }
            return CompletedTask;
        }

        private IHttpActionResult GetActionResultByResponceMessage(ApiController controller, HttpResponseMessage responce)
        {
            var responceMessageMethod = controller.GetType()
                .GetMethod("ResponseMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            return (IHttpActionResult) responceMessageMethod.Invoke(controller, new object[] { responce });
        }
    }
}