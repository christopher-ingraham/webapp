using Microsoft.AspNetCore.Mvc.Filters;
using DA.WI.NSGHSM.Core.Extensions;

namespace DA.WI.NSGHSM.Api.Extensions
{
    public static class FilterContextExtensions
    {
        public static string GetRouteDataForLog(this FilterContext me)
        {
            return me
                ?.RouteData
                ?.Values?.ToJson()
                .ToStringWithNullOrEmptyDescription();
        }

        public static string GetUserNameForLog(this FilterContext me)
        {
            return me
                ?.HttpContext
                ?.User?.Identity?.Name
                .ToStringWithNullOrEmptyDescription();
        }

        public static string GetResponseStatusCodeForLog(this FilterContext me)
        {
            var response = me
                ?.HttpContext
                ?.Response;

            string statusCodeForLog = response != null ? response.StatusCode.ToString() : null;

            return statusCodeForLog
                .ToStringWithNullOrEmptyDescription();
        }
    }
}
