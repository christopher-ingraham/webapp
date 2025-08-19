using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DA.WI.NSGHSM.IdentityServer.Models;

namespace DA.WI.NSGHSM.IdentityServer.Data
{
    // Database Context
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.Department).HasMaxLength(100);
            });

            // Seed default roles
            SeedRoles(builder);

            // Seed default admin user
            SeedUsers(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                }
            );
        }

        private static void SeedUsers(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = "1",
                UserName = "admin@nsghsm.com",
                NormalizedUserName = "ADMIN@NSGHSM.COM",
                Email = "admin@nsghsm.com",
                NormalizedEmail = "ADMIN@NSGHSM.COM",
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                Department = "IT",
                IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

            builder.Entity<ApplicationUser>().HasData(adminUser);

            // Assign admin role to admin user
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = "1"
                }
            );
        }
    }

    // DTOs for API
    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Department { get; set; }
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string? Message { get; set; }
        public UserInfo? User { get; set; }
    }

    public class UserInfo
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Department { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public class TokenRequest
    {
        public required string GrantType { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? RefreshToken { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
