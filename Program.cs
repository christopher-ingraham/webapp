using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection; 


var builder = WebApplication.CreateBuilder(args);

// Set console title
Console.Title = "IdentityServer4";

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File("./logs/identityserver4_log.txt")
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
        theme: AnsiConsoleTheme.Literate)
    .MinimumLevel.Is(IsDevelopment() ? LogEventLevel.Debug : LogEventLevel.Information)
    .CreateLogger();

// Tell the host to use Serilog
builder.Host.UseSerilog();

// Add services
builder.Services.AddControllersWithViews();
// builder.Services.AddRazorPages(); // if using Razor Pages

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// app.MapRazorPages(); // if using Razor Pages

app.Run();

static bool IsDevelopment()
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var isProduction = string.IsNullOrEmpty(env) || string.Equals(env, "Production", StringComparison.OrdinalIgnoreCase);
    return !isProduction;
}
