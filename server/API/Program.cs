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
        DotNetEnv.Env.Load();
        
        // ===================== * DB CONNECTIONS * ===================== //
        
        // builder.Configuration
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //     .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
        //     .AddEnvironmentVariables();
        //
        // var appDbConnectionString = builder.Configuration.GetConnectionString("DbConnectionString")
        //     .Replace("{DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST"))
        //     .Replace("{DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME"))
        //     .Replace("{DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
        //     .Replace("{DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"))
        //     .Replace("{DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT"));
        //
        // builder.Configuration["ConnectionStrings:DbConnectionString"] = appDbConnectionString;
        
        
        // builder.Configuration
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //     .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
        //     .AddEnvironmentVariables();
        //
        // var appDbConnectionString = builder.Configuration.GetConnectionString("AppDb")
        //     .Replace("{DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST")!)
        //     .Replace("{DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME")!)
        //     .Replace("{DB_USER}", Environment.GetEnvironmentVariable("DB_USER")!)
        //     .Replace("{DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD")!)
        //     .Replace("{DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT")!);
        //
        // builder.Configuration["ConnectionStrings:AppDb"] = appDbConnectionString;
        //
        // builder.Services.AddDbContext<LotteryContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppDb")));
        
        
        
        
        // ===================== * -- * ===================== //
        //builder.AddPgContainer();
        // var options = builder.Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>()!;

        //DB CONTEXT INIT
        /*
        builder.Services.AddDbContext<>(config =>
        {
            config.UseNpgsql(options.DbConnectionString);
            config.EnableSensitiveDataLogging();
        });
        */
        //builder.Services.AddScoped<IHospitalService, HospitalService>();

        #region Security
        //RASMUS SECIURITY SECTION 
        
        /*
        builder
            .Services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<HospitalContext>();
        builder
            .Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o => { o.TokenValidationParameters = JwtTokenClaimService.ValidationParameters(options); });
        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .Build();

        });
        builder.Services.AddScoped<ITokenClaimsService, JwtTokenClaimService>();
        */
        #endregion
        
        // ===================== * SCOPES * ===================== //
        builder.Services.AddDbContext<LotteryContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer(); // Add this line
        // I DONT KNOW WHAT ABOUT THIS PART, SOMETHING WITH SECURITY
        
        /*
        builder.Services.AddOpenApiDocument(configuration =>
        {
            {
                configuration.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Scheme = "Bearer ",
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });
                //configuration.AddTypeToSwagger<T>(); //If you need to add some type T to the Swagger known types
                configuration.DocumentProcessors.Add(new MakeAllPropertiesRequiredProcessor());

                configuration.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            }
        });
        */

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseOpenApi();
        app.UseSwaggerUi();
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                context.Request.Headers["Authorization"] = string.Empty;
            }
            await next();
        });
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors(config => config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

       
        app.MapControllers();
        app.UseStatusCodePages();
        //CREATING SCOPES 
        /*
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!roleManager.RoleExistsAsync(Role.Reader).GetAwaiter().GetResult())
            {
                 roleManager.CreateAsync(new IdentityRole(Role.Reader)).GetAwaiter().GetResult();
            }            File.WriteAllText("current_db.sql", context.Database.GenerateCreateScript());
        }
        */
        app.Run();
    }
}