using DA.WI.NSGHSM.Api.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DA.WI.NSGHSM.Api.Filters
{
    public class LogActionStartingFilter : IActionFilter
    {
        private ILogger<LogActionStartingFilter> logger;

        public LogActionStartingFilter(ILogger<LogActionStartingFilter> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var routeData = context.GetRouteDataForLog();
            var userName = context.GetUserNameForLog();

            logger.LogInformation($"ACTION STARTING: {routeData}|USER: {userName}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
