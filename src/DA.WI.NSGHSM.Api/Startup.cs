using DA.WI.NSGHSM.Api.Services;
using DA.WI.NSGHSM.Core.App;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Core.Helpers;
using DA.WI.NSGHSM.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using DA.WI.NSGHSM.Logic.Messaging;
using DA.WI.NSGHSM.Logic.PlantOverview;

namespace DA.WI.NSGHSM.Api
{
    public class Startup
    {
        private readonly Configuration config;
        private readonly IHostingEnvironment env;

        public Startup(IConfiguration configService, IHostingEnvironment env, ILogger<Startup> logger)
        {
            this.config = ReadConfiguration(configService, env, logger);
            this.env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAppServices(config)
                .AddCors(config)
                .AddMvc(config, this.env.IsDevelopment())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddAuthentication(config, env);

             services
                .AddRoleRequiredAuthorization();

            // references:
            // https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.2&tabs=visual-studio#options
            // https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.2&tabs=visual-studio#port-configuration
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 443;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, StartupManager startupManager, 
            ILogger<Startup> logger, 
            MessagingLogic messagingLogic,
            RoughingMillDataLogic rmDataLogic,
            FinishingMillDataLogic fmDataLogic
            // CoolingDowncoilersDataLogic cdDataLogic
            )
        {
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings() {
                Formatting = Newtonsoft.Json.Formatting.Indented,
            };
            logger.LogInformation($"Environment: {env.ToJson(settings)}");

            if (env.IsDevelopment() == true)
            {
                app.UseDeveloperExceptionPage();
                //DEBUG
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            } else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(Constants.Api.Policies.CorsDefault);

            if (env.IsDevelopment()) {
                // [DEV] bypass authorization
            } else {
                app.UseHttpsRedirection();
            }
            app.UseAuthentication();
            app.UseMvc();

            startupManager.Execute();
        }

        private static Configuration ReadConfiguration(IConfiguration configurationService, IHostingEnvironment env, ILogger<Startup> logger)
        {
            var configuration = new Configuration();
            configurationService.Bind(configuration);

            ProcessConfiguration(env, configuration, logger);
            ValidateLanguages(configuration, logger);
            ValidateIsDefaultLanguageCodeExist(configuration, logger);

            if (env.IsDevelopment() == true)
            {
                Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings() {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                };
                logger.LogInformation($"Configuration: {configuration.ToJson(settings)}");
            }

            return configuration;
        }

        private static void ValidateLanguages(Configuration configuration, ILogger<Startup> logger)
        {
            List<string> incorrectLanguageCode = new List<string>();
            configuration.Application.Locales.Keys.ToList().ForEach((key) =>
            {
                if (key.IsCodeValid() == false)
                {
                    incorrectLanguageCode.Add(key);
                    logger.LogInformation($"Incorrect language code : {key} check appsettings.json file section Languages. Example of correct format is en-US");
                }
            });

            incorrectLanguageCode.ForEach((key) => configuration.Application.Locales.Remove(key));
        }

        // this function checks if there is exactly 1 language code with IsDefault set to true. If no default language code is found this method
        // gets the first language code and set it as default
        private static void ValidateIsDefaultLanguageCodeExist(Configuration configuration, ILogger<Startup> logger)
        {
            const string noDefaultLocaleCode = "No locale code is set as Default. The first language code is set as default by the application.Check file appsettings.json";
            var LanguConfigIsDefoultList = configuration.Application.Locales.Values.Where((LanguConfig) => LanguConfig.IsDefault == true);

            if (LanguConfigIsDefoultList.Count() == 0)
            {
                configuration.Application.Locales.Values.First().IsDefault = true;
                logger.LogInformation(noDefaultLocaleCode);
            }

            else if (LanguConfigIsDefoultList.Count() > 1)
            {
                configuration.Application.Locales.Values.ToList().ForEach((LanguConfig) => LanguConfig.IsDefault = false);
                configuration.Application.Locales.Values.First().IsDefault = true;
                logger.LogInformation(noDefaultLocaleCode);
            }
        }

        private static void ProcessConfiguration(IHostingEnvironment env, Configuration configuration, ILogger<Startup> logger)
        {
            configuration.EnvironmentName = configuration.EnvironmentName ?? env.EnvironmentName;
            configuration.RootFolder = configuration.RootFolder ?? env.ContentRootPath;

            configuration.Application = configuration.Application ?? new Configuration.ApplicationConfig();
            configuration.Application.Name = configuration.Application.Name ?? "NSGHSM";

            configuration.Development =  configuration.Development ?? new Configuration.DevelopmentConfig();
            //Properties inside Project Package
            Assembly targetAssembly = Assembly.GetEntryAssembly();


            configuration.Application.Version = targetAssembly.GetVersion();
            configuration.Application.Environment = env.EnvironmentName;

            if (string.IsNullOrWhiteSpace(configuration.Application.TemplateVersion) == false)
            {
                logger.LogInformation($"The information about the Template Version shoud be inserted in Constants.Api.DAWITemplateVersion");
            }
            configuration.Application.TemplateVersion = Constants.Api.DAWITemplateVersion;

            if (string.IsNullOrWhiteSpace(configuration.Application.Copyright) == true)
            {
                configuration.Application.Copyright = targetAssembly.GetCopyright();
            }
            if (string.IsNullOrWhiteSpace(configuration.Application.Customer) == true)
            {
                configuration.Application.Customer = targetAssembly.GetCompany();
            }
            if (string.IsNullOrWhiteSpace(configuration.Application.Description) == true)
            {
                configuration.Application.Description = targetAssembly.GetDescription();
            }

            configuration.Cors.AllowedOrigins = InitHelper.EndPointsBaseURLRelpacer(configuration.Endpoints.WebUrl, configuration.Cors.AllowedOrigins, "[WebUrl]");
            configuration.IdentityServer.Authority = InitHelper.EndPointsBaseURLRelpacer(configuration.Endpoints.IdentityServerUrl, configuration.IdentityServer.Authority, "[IdentityServerUrl]");

        }

    }
}
