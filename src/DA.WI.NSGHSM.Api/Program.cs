using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using System;
using Microsoft.Extensions.Logging;

namespace DA.WI.NSGHSM.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "DA.WI.NSGHSM.Api";

            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("Starting DA.WI.NSGHSM.Api...");

                CreateWebHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch (Exception e)
            {
                logger.Error(e, "DA.WI: stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                {
                    // NLog and ASP.Net Core 2 reference: https://github.com/NLog/NLog.Web/wiki/Getting-started-with-ASP.NET-Core-2

                    // remove standard standard providers (console & debug)
                    //builder.ClearProviders(); //We want to let the standard logging available

                    // let NLog manage min log levels
                    builder.SetMinimumLevel(LogLevel.Trace);

                })
                .UseStartup<Startup>()
                .UseNLog(); // NLog: setup NLog for Dependency injection
    }
}
