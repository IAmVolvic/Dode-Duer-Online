using System.Text.Json.Serialization;
using API.ActionFilters;
using API.Automation;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Service.Security;
using Service.Services;
using Service.Services.Interfaces;

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

        app.UseForwardedHeaders(
            new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        
        ConfigureMiddleware(app);
        
        using (var scope = app.Services.CreateScope())
        {
            var adminUser = scope.ServiceProvider.GetRequiredService<AdminUser>();
            adminUser.RegisterAdminUser();
            
            var gameStart = scope.ServiceProvider.GetRequiredService<GameStart>();
            gameStart.StartGame();
        }

        
    
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
            options.UseNpgsql(Environment.GetEnvironmentVariable("TestDb") ?? builder.Configuration.GetConnectionString("DefaultConnection")));

        // ===================== * CONTROLLERS & MVC * ===================== //
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.AddService<AuthenticatedFilter>();
        });
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilter>(); // This registers the exception filter
        });
        
        // ===================== * REPOSITORIES & SERVICES * ===================== //
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddScoped<EmailService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<IPriceRepository, PriceRepository>();
        builder.Services.AddScoped<IBoardRepository, BoardRepository>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        builder.Services.AddScoped<IPriceService, PriceService>();
        builder.Services.AddScoped<IBoardService, BoardService>();
        builder.Services.AddScoped<IWinnersRepository, WinnersRepository>();
        builder.Services.AddScoped<IWinnersService, WinnersService>();
        builder.Services.AddScoped<IJWTManager, JWTManager>();
        builder.Services.AddScoped<AdminUser>();
        builder.Services.AddScoped<GameStart>();
        builder.Services.AddScoped<AuthenticatedFilter>();
        
        
        // ===================== * MVC & API SUPPORT * ===================== //
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer(); // Support for API Explorer
        builder.Services.AddOpenApiDocument();
        builder.Services.AddSwaggerGen(c =>
        {
            // Register the Enum Schema Filter here
            c.SchemaFilter<EnumSchemaFilter>(); // This ensures that enums are displayed as strings
        });
        
        // ===================== * CORS SETUP * ===================== //
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy =>
            {
                // Replace with the exact origin of your front-end application
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // Allows credentials (cookies, headers, etc.)
            });
        });
    }

    
    private static void ConfigureMiddleware(WebApplication app)
    {
        // ===================== * MIDDLEWARE SETUP * ===================== //
        app.UseHttpsRedirection();
        app.UseRouting();

        // ===================== * SWAGGER (API Documentation) * ===================== //
        app.UseOpenApi();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            c.RoutePrefix = string.Empty;
        });
        
        
        // ===================== * CORS CONFIGURATION * ===================== //
        app.UseCors("AllowSpecificOrigin");
        
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