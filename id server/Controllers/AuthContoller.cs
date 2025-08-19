using DA.WI.NSGHSM.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DA.WI.NSGHSM.IdentityServer.Models;

namespace DA.WI.NSGHSM.IdentityServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.LoginAsync(request);

            if (result.Success)
            {
                _logger.LogInformation("User {Email} logged in successfully", request.Email);
                return Ok(result);
            }

            _logger.LogWarning("Failed login attempt for {Email}", request.Email);
            return Unauthorized(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterAsync(request);

            if (result.Success)
            {
                _logger.LogInformation("User {Email} registered successfully", request.Email);
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _userService.RefreshTokenAsync(refreshToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user context");
            }

            var result = await _userService.LogoutAsync(userId);
            return result ? Ok(new { message = "Logged out successfully" }) : BadRequest("Logout failed");
        }

        [HttpGet("userinfo")]
        [Authorize]
        public async Task<ActionResult<UserInfo>> GetUserInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user context");
            }

            var userInfo = await _userService.GetUserInfoAsync(userId);
            return userInfo != null ? Ok(userInfo) : NotFound();
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult GetCurrentUser()
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(new
            {
                Identity = User.Identity?.Name,
                Claims = userClaims
            });
        }
    }
}
