using DA.WI.NSGHSM.IdentityServer.Services;
using DA.WI.NSGHSM.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DA.WI.NSGHSM.IdentityServer.Controllers
{
    [ApiController]
    [Route("connect")]
    public class ConnectController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<ConnectController> _logger;

        public ConnectController(
            IUserService userService,
            ITokenService tokenService,
            ILogger<ConnectController> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("token")]
        public async Task<ActionResult> Token([FromForm] TokenRequest request)
        {
            try
            {
                if (request.GrantType == "password")
                {
                    if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                    {
                        return BadRequest(new { error = "invalid_request", error_description = "Username and password are required" });
                    }

                    var loginRequest = new LoginRequest
                    {
                        Email = request.Username,
                        Password = request.Password,
                        RememberMe = false
                    };

                    var result = await _userService.LoginAsync(loginRequest);

                    if (result.Success && !string.IsNullOrEmpty(result.Token))
                    {
                        return Ok(new
                        {
                            access_token = result.Token,
                            token_type = "Bearer",
                            expires_in = 86400, // 24 hours
                            scope = "openid profile NSGHSM-api"
                        });
                    }

                    return BadRequest(new { error = "invalid_grant", error_description = "Invalid username or password" });
                }

                return BadRequest(new { error = "unsupported_grant_type", error_description = "Only password grant type is supported" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in token endpoint");
                return StatusCode(500, new { error = "server_error", error_description = "Internal server error" });
            }
        }

        [HttpGet("authorize")]
        public ActionResult Authorize(
            [FromQuery] string client_id,
            [FromQuery] string redirect_uri,
            [FromQuery] string response_type,
            [FromQuery] string scope,
            [FromQuery] string state = "")
        {
            // For implicit flow - redirect to login page with parameters
            var loginUrl = $"/Account/Login?client_id={client_id}&redirect_uri={Uri.EscapeDataString(redirect_uri)}&response_type={response_type}&scope={Uri.EscapeDataString(scope)}&state={state}";
            return Redirect(loginUrl);
        }

        [HttpGet("userinfo")]
        [Authorize]
        public async Task<ActionResult> UserInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var userInfo = await _userService.GetUserInfoAsync(userId);
            if (userInfo == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                sub = userInfo.Id,
                name = $"{userInfo.FirstName} {userInfo.LastName}".Trim(),
                given_name = userInfo.FirstName,
                family_name = userInfo.LastName,
                email = userInfo.Email,
                role = userInfo.Roles,
                department = userInfo.Department
            });
        }

        [HttpPost("endsession")]
        [Authorize]
        public async Task<ActionResult> EndSession([FromQuery] string post_logout_redirect_uri)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await _userService.LogoutAsync(userId);
            }

            if (!string.IsNullOrEmpty(post_logout_redirect_uri))
            {
                return Redirect(post_logout_redirect_uri);
            }

            return Ok();
        }
    }
}
