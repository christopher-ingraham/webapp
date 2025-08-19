using DA.WI.NSGHSM.IdentityServer.Configuration;
using DA.WI.NSGHSM.IdentityServer.Services;
using DA.WI.NSGHSM.IdentityServer.Data;
using DA.WI.NSGHSM.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Console title
Console.Title = "NSGHSM Identity Server - .NET 8";

try
{
    Log.Information("Starting NSGHSM Identity Server - .NET 8");

    // Add services to the container
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Configure CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins", policy =>
        {
            var allowedOrigins = builder.Configuration.GetSection("Endpoints:WebUrl").Get<string[]>() ?? Array.Empty<string>();
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });

    // Choose between Duende IdentityServer or ASP.NET Core Identity + JWT
    var identityServerType = builder.Configuration.GetValue<string>("IdentityServerType", "AspNetCore");

    if (identityServerType == "Duende")
    {
        // Option 1: Configure Duende IdentityServer (requires commercial license)
        try
        {
            builder.Services.ConfigureDuendeIdentityServer(builder.Configuration);
        }
        catch (NotImplementedException ex)
        {
            Log.Warning("Duende IdentityServer not available: {Message}. Falling back to ASP.NET Core Identity + JWT", ex.Message);
            builder.Services.ConfigureAspNetCoreIdentityWithJwt(builder.Configuration);
        }
    }
    else
    {
        // Option 2: Configure ASP.NET Core Identity with JWT (recommended)
        builder.Services.ConfigureAspNetCoreIdentityWithJwt(builder.Configuration);
    }

    // Add custom services
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ITokenService, TokenService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "NSGHSM Identity Server API v1");
            c.RoutePrefix = "swagger";
        });
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseCors("AllowSpecificOrigins");

    app.UseAuthentication();
    app.UseAuthorization();

    if (identityServerType == "Duende")
    {
        // For Duende IdentityServer (only if properly configured)
        try
        {
            app.UseIdentityServer();
        }
        catch
        {
            Log.Warning("Duende IdentityServer not configured properly. Using ASP.NET Core Identity only.");
        }
    }

    app.MapControllers();
    app.MapFallbackToFile("index.html");

    // Ensure database is created
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        context?.Database.EnsureCreated();
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}