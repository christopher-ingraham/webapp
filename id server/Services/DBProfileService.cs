using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Dto._Core.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace DA.WI.NSGHSM.IdentityServer.Services
{
    public class DBProfileService : IProfileService
    {
        private readonly IUserStore store;
        private readonly ILogger<DBProfileService> logger;

        public DBProfileService(IUserStore store, ILogger<DBProfileService> logger)
        {
            this.store = store;
            this.logger = logger;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                var userName = context.Subject.Identity.Name;

                var user = store.FindByUsernameOrDefault(userName);
                if (user != null)
                {
                    var userClaims = new List<Claim>();
                    userClaims.Add(CreateUserNameClaim(user));
                    userClaims.AddRange(CreateUserRoleClaims(user));

                    context.AddRequestedClaims(userClaims);
                }
            }
            catch (Exception ex)
            {
                // log and swallow because an unhandled exception here could cause an IdentityServer fatal error
                logger.LogError(ex, ex.Message);

            }

            return Task.FromResult(0);
        }


        public Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                var userId = GetUserIdFromSubjectOrDefault(context.Subject);

                context.IsActive = (userId != null)
                    ? store.IsUserActive(userId.Value)
                    : false;
            }
            catch (Exception ex)
            {
                // log and swallow because an unhandled exception here could cause an IdentityServer fatal error
                logger.LogError(ex, ex.Message);
                context.IsActive = false;
            }

            return Task.FromResult(0);
        }

        private static long? GetUserIdFromSubjectOrDefault(ClaimsPrincipal subject)
        {
            var subClaim = GetSubClaimOrDefault(subject);
            if (subClaim == null)
                return null;

            long? userId = ConvertToUserIdOrDefault(subClaim);

            return userId;            
        }

        private static long? ConvertToUserIdOrDefault(Claim subClaim)
        {
            long userId = 0;
            if (!long.TryParse(subClaim.Value, out userId))
                return null;

            return userId;
        }

        private static Claim GetSubClaimOrDefault(ClaimsPrincipal subject)
        {
            if (subject == null)
                return null;

            return subject
                .Claims
                .FirstOrDefault(_ => _.Type == JwtClaimTypes.Subject);
        }

        private Claim CreateUserNameClaim(UserDto user)
        {
            return new Claim(JwtClaimTypes.Name, user.UserName);
        }

        private static Claim[] CreateUserRoleClaims(UserDto user)
        {
            return user?.Roles
                .Select(_ => new Claim(JwtClaimTypes.Role, _))
                .ToArrayOrEmpty();
        }
    }
}
