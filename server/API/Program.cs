using API.Extensions;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.Generation.Processors.Security;
using Service;
using Service.Security;
using Service.Services;
using Service.Services.Interfaces;
using Service.Validators;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ===================== * ENVIRONMENT VARIABLES * ===================== //
        DotNetEnv.Env.Load();

        // ===================== * CONFIGURATION SETUP * ===================== //
        ConfigureConfiguration(builder);

        // ===================== * DEPENDENCY INJECTION * ===================== //
        ConfigureServices(builder);

        // ===================== * BUILD & MIDDLEWARE PIPELINE * ===================== //
        var app = builder.Build();
        ConfigureMiddleware(app);

        app.Run();
    }

    private static void ConfigureConfiguration(WebApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        // Build the connection string dynamically using environment variables
        var appDbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            .Replace("{DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST"))
            .Replace("{DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME"))
            .Replace("{DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
            .Replace("{DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"))
            .Replace("{DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT"));

        builder.Configuration["ConnectionStrings:DefaultConnection"] = appDbConnectionString;
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        // ===================== * DATABASE CONTEXT * ===================== //
        builder.Services.AddDbContext<LotteryContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // ===================== * REPOSITORIES & SERVICES * ===================== //
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        // ===================== * MVC & API SUPPORT * ===================== //
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer(); // Support for API Explorer
        builder.Services.AddOpenApiDocument();
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        // ===================== * MIDDLEWARE SETUP * ===================== //
        app.UseHttpsRedirection();
        app.UseRouting();

        // ===================== * SWAGGER (API Documentation) * ===================== //
        app.UseOpenApi();
        app.UseSwaggerUi();
        
        // ===================== * AUTHENTICATION & AUTHORIZATION * ===================== //
        app.UseAuthentication();
        app.UseAuthorization();

        // ===================== * CORS CONFIGURATION * ===================== //
        app.UseCors(config => config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

        // ===================== * STATUS CODE HANDLING & ROUTES * ===================== //
        app.MapControllers();
        app.UseStatusCodePages();
    }
}