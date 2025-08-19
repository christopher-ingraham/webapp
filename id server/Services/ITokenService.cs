using System.Security.Claims;
using DA.WI.NSGHSM.IdentityServer.Models;
namespace DA.WI.NSGHSM.IdentityServer.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(ApplicationUser user, IList<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
