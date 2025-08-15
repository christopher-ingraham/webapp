using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace DA.WI.NSGHSM.Web
{
    public class Startup
    {
        // <base href="/NSGHSM/web/">
        private string PathBase = "/NSGHSM/web";

        // local file system path, for Angular SPA
        private const string ClientAppDist = "ClientApp/dist";

        // CORS policy
        private const string CorsPolicy = "MyPolicy";


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors((o) => o.AddPolicy(CorsPolicy, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddSpaStaticFiles((configuration) =>
            {
                configuration.RootPath = ClientAppDist;
            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() == true || env.IsEnvironment("DevelopmentAuth"))
            {
                app.UseDeveloperExceptionPage();
                app.UseStaticFiles();
                app.UseSpaStaticFiles();

                app.UsePathBase(PathBase);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days.
                // You may want to change this for production scenarios,
                // see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            //            app.UsePathBase(PathBase);

            app.UseMvc((routes) =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseCors(CorsPolicy);

            if (env.IsDevelopment() == true || env.IsEnvironment("DevelopmentAuth"))
            {
                app.UseSpa((spa) =>
                {
                    // To learn more about options for serving an Angular SPA from ASP.NET Core,
                    // see https://go.microsoft.com/fwlink/?linkid=864501

                    spa.Options.SourcePath = "ClientApp";

                    string root = Path.Combine(Directory.GetCurrentDirectory(), spa.Options.SourcePath);
                    spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(root)
                    };
                    if (env.IsDevelopment() == true)
                    {
                        spa.UseAngularCliServer(npmScript: "start-dev");
                    }
                    if (env.IsEnvironment("DevelopmentAuth"))
                    {
                        spa.UseAngularCliServer(npmScript: "start-devauth");
                    }
                });
            }
            else
            {

                app.Map(PathBase, frontendApp =>
                {
                    frontendApp.UseSpaStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), ClientAppDist))
                    });
                    frontendApp.UseSpa((spa) =>
                    {
                        // To learn more about options for serving an Angular SPA from ASP.NET Core,
                        // see https://go.microsoft.com/fwlink/?linkid=864501

                        spa.Options.SourcePath = "ClientApp";
                        spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                        {
                            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), ClientAppDist))
                        };
                       
                    });
                });
            }


        }
    }
}
