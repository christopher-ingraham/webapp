using DA.WI.NSGHSM.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DA.WI.NSGHSM.IdentityServer.Services
{
    // Application User extending IdentityUser
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null || !user.IsActive)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid email or password."
                    };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!result.Succeeded)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid email or password."
                    };
                }

                var roles = await _userManager.GetRolesAsync(user);
                var token = _tokenService.GenerateJwtToken(user, roles);

                // Update last login
                user.LastLoginAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                return new AuthResponse
                {
                    Success = true,
                    Token = token,
                    Expiration = DateTime.UtcNow.AddHours(24),
                    User = new UserInfo
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Department = user.Department,
                        Roles = roles.ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Email}", request.Email);
                return new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during login."
                };
            }
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                if (request.Password != request.ConfirmPassword)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Passwords do not match."
                    };
                }

                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "User with this email already exists."
                    };
                }

                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Department = request.Department,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new AuthResponse
                    {
                        Success = false,
                        Message = $"Registration failed: {errors}"
                    };
                }

                // Assign default User role
                await _userManager.AddToRoleAsync(user, "User");

                var roles = await _userManager.GetRolesAsync(user);
                var token = _tokenService.GenerateJwtToken(user, roles);

                return new AuthResponse
                {
                    Success = true,
                    Token = token,
                    Expiration = DateTime.UtcNow.AddHours(24),
                    User = new UserInfo
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Department = user.Department,
                        Roles = roles.ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {Email}", request.Email);
                return new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during registration."
                };
            }
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            // Implementation for refresh token logic
            // This would typically involve validating the refresh token and generating a new access token
            await Task.CompletedTask;
            return new AuthResponse { Success = false, Message = "Refresh token not implemented yet." };
        }

        public async Task<bool> LogoutAsync(string userId)
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout for user {UserId}", userId);
                return false;
            }
        }

        public async Task<UserInfo?> GetUserInfoAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || !user.IsActive)
                {
                    return null;
                }

                var roles = await _userManager.GetRolesAsync(user);

                return new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Department = user.Department,
                    Roles = roles.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user info for {UserId}", userId);
                return null;
            }
        }
    }
}
