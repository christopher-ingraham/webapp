using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace DA.WI.NSGHSM.Api._Core
{
    public static class RoleName
    {
        public static string All = "All";
        public static string Restricted = "Restricted";
        public static string ReadOnly = "ReadOnly";
    }

    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
        {
            Roles = String.Join(",", roles);
        }
    }

    public class AuthorizeRoleAllAttribute : AuthorizeRolesAttribute
    {
        public AuthorizeRoleAllAttribute() : base(RoleName.All) { }
    }

    public class AuthorizeRoleRestrictedAttribute : AuthorizeRolesAttribute
    {
        public AuthorizeRoleRestrictedAttribute() : base(RoleName.Restricted) { }
    }

    public class AuthorizeRoleReadOnlyAttribute : AuthorizeRolesAttribute
    {
        public AuthorizeRoleReadOnlyAttribute() : base(RoleName.ReadOnly) { }
    }

    public class AuthorizeAllRolesAttribute : AuthorizeRolesAttribute
    {
        public AuthorizeAllRolesAttribute() : base(RoleName.All, RoleName.Restricted, RoleName.ReadOnly) { }
    }
    
    public class AuthorizeRolesAllRestrictedAttribute : AuthorizeRolesAttribute
    {
        public AuthorizeRolesAllRestrictedAttribute() : base(RoleName.All, RoleName.Restricted) { }
    }
}