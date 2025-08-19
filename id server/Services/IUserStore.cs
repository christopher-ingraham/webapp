using DA.WI.NSGHSM.Dto._Core.Configuration;

namespace DA.WI.NSGHSM.IdentityServer.Services
{
    public interface IUserStore
    {
        UserDto FindByUsernameOrDefault(string username);

        bool ValidateCredentials(string username, string password);

        bool IsUserActive(long userId);
    }
}