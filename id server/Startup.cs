// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using DA.WI.DirectoryServices;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Core.Helpers;
using DA.WI.NSGHSM.IdentityServer.Services;
using DA.WI.NSGHSM.Repo;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Hosting;

using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System;

namespace DA.WI.NSGHSM.IdentityServer
{
    public class Startup
    {
        public readonly IWebHostEnvironment environment;
        public readonly IConfiguration configurationService;
        public readonly ILogger<Startup> logger;

        private readonly bool isTestUserStoreEnabled;

        private readonly SigningCredentialConfig signingCredentialConfig = new SigningCredentialConfig();
        private readonly DA.WI.NSGHSM.Core.App.Configuration appConfig = new DA.WI.NSGHSM.Core.App.Configuration();

        private bool isSigningCredentialInitialized = false;

        public Startup(IWebHostEnvironment environment, IConfiguration configurationService, ILogger<Startup> logger)
        {
            this.environment = environment;
            this.configurationService = configurationService;
            this.logger = logger;

            this.isTestUserStoreEnabled = configurationService.GetValue<bool>("IsTestUserStoreEnabled", false);

            configurationService.Bind("SigningCredential", this.signingCredentialConfig);
            configurationService.Bind(this.appConfig);


        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            IdentityModelEventSource.ShowPII = true; //To show detail of error and see the problem

            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
                options.AuthenticationDisplayName = "Windows";
            });

            var identityServerConfiguration = new IdentityServerConfiguration();
            configurationService.Bind(identityServerConfiguration);
            // URL Base Placeholders replacement
            foreach (Client client in identityServerConfiguration.Clients)
            {
                client.RedirectUris = InitHelper.EndPointsBaseURLRelpacer(this.appConfig.Endpoints.WebUrl, client.RedirectUris.ToArrayOrEmpty(), "[WebUrl]");
                client.PostLogoutRedirectUris = InitHelper.EndPointsBaseURLRelpacer(this.appConfig.Endpoints.WebUrl, client.PostLogoutRedirectUris.ToArrayOrEmpty(), "[WebUrl]");
                client.AllowedCorsOrigins = InitHelper.EndPointsBaseURLRelpacer(this.appConfig.Endpoints.WebUrl, client.AllowedCorsOrigins.ToArrayOrEmpty(), "[WebUrl]");
            }


            var builder = services
                .AddIdentityServer((IdentityServerOptions options) =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.Authentication.CookieLifetime = TimeSpan.FromDays(30);
                    options.Authentication.CookieSlidingExpiration = true;
                    options.PublicOrigin = configurationService.GetValue<string>("PublicOrigin", null);
                    //options.IssuerUri = "http://localhost:5013";                    
                    options.IssuerUri = configurationService.GetValue<string>("IssuerUri", null);
                });
                // .AddInMemoryClients(identityServerConfiguration.Clients);

            builder.AddInMemoryIdentityResources(configurationService.GetSection("IdentityResources"));
            builder.AddInMemoryApiResources(configurationService.GetSection("ApiResources"));
            builder.AddInMemoryClients(identityServerConfiguration.Clients);

            this.isSigningCredentialInitialized = builder.AddSigningCredential(this.signingCredentialConfig, this.logger);

            if (isTestUserStoreEnabled == true)
            {
                builder.AddTestUsers(TestUsers.Users);
                services.AddTransient<IUserStore, TestUserStoreWrapper>();
            }
            else
            {
                services.AddTransient(sp => this.appConfig.JsonClone());
                services.AddTransient<IUserStore, DBUserStore>();
                services.AddRepoServicesForIdentityServer(this.appConfig);
                builder.AddProfileService<DBProfileService>();
            }

            services.AddPrincipalContextManager(this.configurationService);
        }


        public void Configure(IApplicationBuilder app)
        {
            this.logger.LogInformation($"Environment: {this.environment.ToJson()}");

            if(!this.isSigningCredentialInitialized)
            {
                throw new Exception($"Need to configure key material! Please configure correctly the SigningCredential section in app.{this.environment.EnvironmentName.ToLower()}.settings");
            }

            if (environment.IsDevelopment() == true)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            // necessary to avoid Content-Security-Policy error on Kendo UI "all.css"
            // that contains a font-face of type "data:"
            // for reference: https://joonasw.net/view/csp-in-aspnet-core
            app.UseCsp(csp =>
            {
                csp.AllowFonts
                    .FromSelf()
                    .From("data:");
            });

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }

    internal class IdentityServerConfiguration
    {
        public IdentityServerConfiguration()
        {
        }

        public List<Client> Clients { get; set; }
    }

    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPrincipalContextManager(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<PrincipalContextOptions<PrincipalContextManager>>(opt =>
                config.Bind("ADConnection", opt)
            );

            return services.AddTransient<PrincipalContextManager>();
        }
    }

    internal class SigningCredentialConfig
    {
        public bool IsDeveloperSigningCredentialEnabled { get; set; }
        public string KeyFilePath { get; set; }
        public string KeyFilePassword { get; set; }
    }

    internal static class BuilderExtensions
    {
        // inspired by http://amilspage.com/signing-certificates-idsv4/
        public static bool AddSigningCredential(
            this IIdentityServerBuilder builder,
            SigningCredentialConfig options,
            ILogger logger)
        {
            if(options.IsDeveloperSigningCredentialEnabled == true)
            {
                builder.AddDeveloperSigningCredential();
                return true;
            }

            if (File.Exists(options.KeyFilePath) == true)
            {
                logger.LogDebug($"SigninCredentialExtension adding key from file {options.KeyFilePath}");

                // You can simply add this line in the Startup.cs if you don't want an extension. 
                // This is neater though ;)
                builder.AddSigningCredential(new X509Certificate2(options.KeyFilePath, options.KeyFilePassword));
                return true;
            }

            logger.LogError($"SigninCredentialExtension cannot find key file {options.KeyFilePath}");

            return false;
        }
    }
}