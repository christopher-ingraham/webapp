using DA.WI.NSGHSM.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace DA.WI.NSGHSM.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private ILogger<ExceptionFilter> logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;

            Log(exception);

            context.Result = ConvertToActionResult(exception);

            context.ExceptionHandled = true;
        }

        private void Log(Exception exception)
        {
            if (exception is ApplicationException applicationException)
                logger.LogInformation($"Handled application exception: {applicationException.Message}");
            else
                logger.LogError(exception, "Unhandled exception");
        }

        private IActionResult ConvertToActionResult(Exception exception)
        {
            if (exception is NotFoundException)
                return new NotFoundObjectResult(exception.Message);

            if (exception is BadRequestException badRequestException)
                return new BadRequestObjectResult(new
                {
                    badRequestException.Message,
                    badRequestException.badRequestMap
                });

            if (exception is ForbiddenException)
                return new ForbidResult();

            if (exception is UnauthorizedException)
                return new UnauthorizedResult();
            
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
