using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DA.WI.NSGHSM.Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetName(this ClaimsPrincipal me)
        {
            if (me == null) return null;

            return me.Identity?.Name;
        }

        public static string GetEmail(this ClaimsPrincipal me)
        {
            if (me == null) return null;

            return me.FindFirstValue(ClaimTypes.Email);
        }

        public static IEnumerable<string> GetRoles(this ClaimsPrincipal me)
        {
            if (me == null) return null;

            return me
                .FindAll(ClaimTypes.Role)
                .Select(_ => _.Value);
        }

        private static string FindFirstValue(this ClaimsPrincipal me, string type)
        {
            if (me == null) return string.Empty;

            return me.Claims.FirstOrDefault(f => f.Type == type)?.Value;
        }

    }
}
