using DA.WI.NSGHSM.Core.App;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Core.Services;
using DA.WI.NSGHSM.Api.Filters;
using DA.WI.NSGHSM.Logic;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace DA.WI.NSGHSM.Api.Services
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, Configuration configuration)
        {
            services
                .AddTransient(sp => configuration.JsonClone())
                .AddTransient<StartupManager>();

            services.AddLogicServices(configuration);

            return services;
        }

        public static IServiceCollection AddCors(this IServiceCollection services, Configuration configuration)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy(Constants.Api.Policies.CorsDefault, policy =>
                {
                    policy.FromConfig(configuration);
                });
                options.AddPolicy(Constants.Api.Policies.CorsWithContentDisposition, policy =>
                {
                    policy
                        .FromConfig(configuration)
                        .WithExposedHeaders("Content-Disposition"); // content-disposition is *exposed* (and allowed because of AllowAnyHeader)
                });
            });
        }

        public static IMvcBuilder AddMvc(this IServiceCollection services, Configuration config, bool isDevelopment)
        {
            return services
                .AddMvc(o =>
                {
                    o.Filters
                        .AddExceptionFilter()
                        .AddLogFilters(config);

                    if (isDevelopment) {
                        o.Filters.Add(new AllowAnonymousFilter());
                    } else {
                        o.Filters.AddAuthorizationFilter();
                    }
                });
        }

        private static FilterCollection AddAuthorizationFilter(this FilterCollection filters)
        {
            var authorizationFilter = new AuthorizeFilter(Constants.Api.Policies.WhitelistAuthorization);

            filters.Add(authorizationFilter);

            return filters;
        }

        private static FilterCollection AddExceptionFilter(this FilterCollection filters)
        {
            filters.Add(typeof(ExceptionFilter));

            return filters;
        }

        private static FilterCollection AddLogFilters(this FilterCollection filters, Configuration config)
        {
            var isLogActionStartingEnabled = (config.Log?.IsLogActionStarting ?? false);
            if (isLogActionStartingEnabled == true)
            {
                filters.Add(typeof(LogActionStartingFilter));
            }

            var isLogActionFinishedEnabled = (config.Log?.IsLogActionFinishing ?? false);
            if (isLogActionFinishedEnabled == false)
            {
                filters.Add(typeof(LogActionFinishedFilter));
            }

            return filters;
        }

        public static CorsPolicyBuilder FromConfig(this CorsPolicyBuilder policy, Configuration configuration)
        {
            return policy
                    .WithOrigins(configuration?.Cors?.AllowedOrigins ?? new string[] { })
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        }

        public static void AddAuthentication(this IServiceCollection services, Configuration configuration, IHostingEnvironment env)
        {
            var requireHttpsMetadata = !env.IsDevelopment();


            // reference http://docs.identityserver.io/en/release/topics/apis.html#the-identityserver-authentication-handler
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(_ =>
                {
                    // === FOR DEMO ONLY
                    // [DEV]
                    _.RequireHttpsMetadata = false; // requireHttpsMetadata;
                    // SET THIS TO true IN PRODUCTION!

                    // base-address of your identityserver
                    _.Authority = configuration?.IdentityServer?.Authority;

                    // name of the API resource
                    _.Audience = configuration?.IdentityServer?.Audience;

                    // necessary to map User.Identity.Name to claim "name"
                    _.TokenValidationParameters.NameClaimType = JwtClaimTypes.Name;
                });
        }

        public static IServiceCollection AddRoleRequiredAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorization(authOptions =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .RequireAssertion(ctx => ctx.Requirements.Any(_ => _ is RolesAuthorizationRequirement))
                                    .Build();

                authOptions.AddPolicy(Constants.Api.Policies.WhitelistAuthorization, policy);
            });
        }

    }
}
