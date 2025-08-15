using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DA.WI.NSGHSM.Dto._Core.Configuration;
using IdentityModel;
using IdentityServer4.Test;

namespace DA.WI.NSGHSM.IdentityServer.Services
{
    public class TestUserStoreWrapper : IUserStore
    {
        private readonly TestUserStore userStore;

        public TestUserStoreWrapper(TestUserStore userStore = null)
        {
            // if the TestUserStore is not in DI, then we'll just use the global users collection
            // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
            this.userStore = userStore ?? new TestUserStore(TestUsers.Users);
        }

        public UserDto FindByUsernameOrDefault(string username)
        {
            var testUser = userStore.FindByUsername(username);
            if (testUser == null)
                return null;

            return new UserDto
            {
                Id = long.Parse(testUser.SubjectId),
                IsFromActiveDirectory = false,
                Roles = CreateUserRoles(testUser),
                UserName = testUser.Username
            };
        }

        public bool ValidateCredentials(string username, string password)
        {
            return userStore.ValidateCredentials(username, password);
        }

        private static List<string> CreateUserRoles(TestUser testUser)
        {
            return testUser.Claims.Where(_ => _.Type == JwtClaimTypes.Role).Select(_ => _.Value).ToList();
        }

        public bool IsUserActive(long userId)
        {
            return userStore.FindBySubjectId(userId.ToString()) != null;
        }
    }
}
