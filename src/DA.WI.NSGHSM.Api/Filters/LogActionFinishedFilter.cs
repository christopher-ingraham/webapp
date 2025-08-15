using DA.WI.NSGHSM.Api.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DA.WI.NSGHSM.Api.Filters
{
    public class LogActionFinishedFilter : IResultFilter
    {
        private ILogger<LogActionFinishedFilter> logger;

        public LogActionFinishedFilter(ILogger<LogActionFinishedFilter> logger)
        {
            this.logger = logger;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            var routeData = context.GetRouteDataForLog();
            var userName = context.GetUserNameForLog();
            var statusCode = context.GetResponseStatusCodeForLog();

            logger.LogInformation($"ACTION FINISHED: {routeData}|USER: {userName}|STATUS: {statusCode}");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
        }
    }
}
