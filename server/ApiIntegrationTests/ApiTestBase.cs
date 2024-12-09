using System.Net.Http.Headers;
using API;
using DataAccess;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using PgCtx;
using Service;
using Service.Services.Interfaces;
using Service.TransferModels.Responses;

namespace ApiInterationTests;

public class ApiTestBase : WebApplicationFactory<Program>
{
    public ApiTestBase()
    {
        PgCtxSetup = new PgCtxSetup<LotteryContext>();
        Environment.SetEnvironmentVariable("ADMIN_NAME", "TestAdmin");
        Environment.SetEnvironmentVariable("ADMIN_EMAIL", "testadmin@example.com");
        Environment.SetEnvironmentVariable("ADMIN_PHONENUMBER", "12345678");
        Environment.SetEnvironmentVariable("ADMIN_PASSWORD", "ValidPassword123");
        Environment.SetEnvironmentVariable("TestDb", PgCtxSetup._postgres.GetConnectionString());
        
        ApplicationServices = base.Services.CreateScope().ServiceProvider;
        
        // Mock the AuthService
        MockAuthService = new Mock<IAuthService>();
        TestHttpClient = CreateClient();
        TestHttpClient.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");
        Seed().GetAwaiter().GetResult();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the DB context
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<LotteryContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<LotteryContext>(opt =>
            {
                opt.UseNpgsql(PgCtxSetup._postgres.GetConnectionString());
                opt.EnableSensitiveDataLogging(false);
                opt.LogTo(_ => { });
            });

            // Use the mocked AuthService
            services.AddScoped<IAuthService>(_ => MockAuthService.Object);
        });

        return base.CreateHost(builder);
    }

    public async Task Seed()
    {
        var ctx = ApplicationServices.GetRequiredService<LotteryContext>();
        await ctx.SaveChangesAsync();
    }

    #region Properties

    public Mock<IAuthService> MockAuthService { get; private set; }
    public PgCtxSetup<LotteryContext> PgCtxSetup;
    public HttpClient TestHttpClient { get; set; }

    public IServiceProvider ApplicationServices { get; set; }

    #endregion
}
