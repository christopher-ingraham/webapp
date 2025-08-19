using DA.WI.NSGHSM.IdentityServer;
using DA.WI.NSGHSM.IdentityServer.Models;
using DA.WI.NSGHSM.IdentityServer.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using static DA.WI.NSGHSM.Core.App.Configuration;

namespace DA.WI.NSGHSM.IdentityServer.Configuration
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection ConfigureAspNetCoreIdentityWithJwt(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure Entity Framework
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                // Use in-memory database for development
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("IdentityDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseOracle(connectionString));
            }

            // Configure Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Configure JWT Authentication
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new ArgumentNullException("JwtSettings:SecretKey");
            var issuer = jwtSettings["Issuer"] ?? throw new ArgumentNullException("JwtSettings:Issuer");
            var audience = jwtSettings["Audience"] ?? throw new ArgumentNullException("JwtSettings:Audience");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false; // Set to true in production
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Add authorization policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });

            return services;
        }

        public static IServiceCollection ConfigureDuendeIdentityServer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Note: This method requires Duende.IdentityServer NuGet package
            // If you don't have a commercial license, use the ASP.NET Core Identity option instead

            throw new NotImplementedException(
                "Duende IdentityServer requires a commercial license. " +
                "Use ConfigureAspNetCoreIdentityWithJwt instead, or install Duende.IdentityServer package.");

            // Uncomment below if you have Duende IdentityServer license:
            /*
            // Configure Entity Framework
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("IdentityDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseOracle(connectionString));
            }

            // Configure Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure Duende IdentityServer (requires Duende.IdentityServer package)
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                
                options.EmitStaticAudienceClaim = true;
                options.IssuerUri = configuration.GetValue<string>("IssuerUri");
            })
            .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
            .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
            .AddInMemoryClients(IdentityServerConfig.GetClients(configuration))
            .AddAspNetIdentity<ApplicationUser>();

            // Configure signing credential
            var isDevelopment = configuration.GetValue<bool>("SigningCredential:IsDeveloperSigningCredentialEnabled", true);
            
            if (isDevelopment)
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                var keyFilePath = configuration.GetValue<string>("SigningCredential:KeyFilePath");
                var keyFilePassword = configuration.GetValue<string>("SigningCredential:KeyFilePassword");
                
                if (!string.IsNullOrEmpty(keyFilePath) && File.Exists(keyFilePath))
                {
                    builder.AddSigningCredential(new X509Certificate2(keyFilePath, keyFilePassword));
                }
                else
                {
                    throw new FileNotFoundException($"Certificate file not found: {keyFilePath}");
                }
            }

            return services;
            */
        }
    }
}
