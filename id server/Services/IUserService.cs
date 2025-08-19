using DA.WI.NSGHSM.IdentityServer.Models;

namespace DA.WI.NSGHSM.IdentityServer.Services
{
    public interface IUserService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(string userId);
        Task<UserInfo?> GetUserInfoAsync(string userId);
    }
}
