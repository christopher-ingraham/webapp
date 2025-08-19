using DA.WI.NSGHSM.Core.App;
using DA.WI.NSGHSM.Core.Exceptions;
using DA.WI.NSGHSM.Dto._Core.Configuration;
using DA.WI.NSGHSM.IRepo._Core.Configuration;
using DA.WI.NSGHSM.Repo;
using DA.WI.DirectoryServices;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DA.WI.NSGHSM.IdentityServer.Services
{
    public class DBUserStore : IUserStore
    {
        private readonly IUserRepo<ConfigurationDataSource> userRepo;
        private readonly PrincipalContextManager principalContext;

        public DBUserStore(IUserRepo<ConfigurationDataSource> userRepo, PrincipalContextManager principalContext)
        {
            this.userRepo = userRepo;
            this.principalContext = principalContext;
        }

        public UserDto FindByUsernameOrDefault(string username)
        {
            try
            {
                return userRepo.GetUserByName(username);
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        public bool IsUserActive(long userId)
        {
            try
            {
                userRepo.Read(userId);
            }
            catch (NotFoundException)
            {
                return false;
            }

            return true;
        }

        public bool ValidateCredentials(string username, string password)
        {
            UserDto thisUserDto = FindByUsernameOrDefault(username);

            if (thisUserDto == null)
            {
                return false;
            }

            if (principalContext.IsEnabled == true)
            {
                return principalContext.ValidateCredential(username, password);
            }

            string hashedPassword = HashPassword(password);
            return (thisUserDto.Password == hashedPassword);
        }

        private string HashPassword(string password)
        {
            // Here we will insert the hashing code for password

            return password;
        }
    }
}
