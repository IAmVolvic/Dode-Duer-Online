using System.Net.Http.Json;
using System.Net;
using DataAccess;
using DataAccess.Types.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Program = API.Program;
using PgCtx;
using Service.Services.Interfaces;
using Service.TransferModels.Responses;
using Xunit;
using Xunit.Abstractions;

namespace ApiIntegrationTests;

public class GameApiTest : WebApplicationFactory<Program>
{
    private readonly PgCtxSetup<LotteryContext> _pgCtxSetup = new();
    private readonly ITestOutputHelper _output;

    public GameApiTest(ITestOutputHelper output)
    {
        Environment.SetEnvironmentVariable("ADMIN_NAME", "TestAdmin");
        Environment.SetEnvironmentVariable("ADMIN_EMAIL", "testadmin@example.com");
        Environment.SetEnvironmentVariable("ADMIN_PHONENUMBER", "12345678");
        Environment.SetEnvironmentVariable("ADMIN_PASSWORD", "ValidPassword123");
        _pgCtxSetup = new PgCtxSetup<LotteryContext>();
        Environment.SetEnvironmentVariable("TestDb",_pgCtxSetup._postgres.GetConnectionString());
        _output = output;
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1000)]
    public async Task Create_Game_API_Test_Creates_Game_With_Starting_Prize_Pool(int startingPrizePool)
    {
        var guid = new Guid();
        var mockAuthService = new Mock<IAuthService>();
        mockAuthService.Setup(s => s.IsUserAuthenticated(It.IsAny<string>())).Verifiable();
        mockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
            .Returns(new AuthorizedUserResponseDTO() { Id = guid, Name = "TestAdmin", Role = "Admin"  });
        mockAuthService.Setup(s => s.IsUserAuthorized(It.IsAny<string[]>(), It.IsAny<string>())).Verifiable();

        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<IAuthService>(_ => mockAuthService.Object);
                });
            });

        var client = appFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");

        var response = await client.PostAsJsonAsync("/Game/NewGame", startingPrizePool);
        _output.WriteLine($"Response Status Code: {response.StatusCode}");
        _output.WriteLine($"Response Content: {await response.Content.ReadAsStringAsync()}");
        Assert.True(response.StatusCode == HttpStatusCode.OK);

        var returnedGame = await response.Content.ReadFromJsonAsync<GameResponseDTO>();
        Assert.NotNull(returnedGame);

        var gameInDb = _pgCtxSetup.DbContextInstance.Games.First(g => g.Id == returnedGame.Id);
        Assert.NotNull(gameInDb);

        Assert.True(gameInDb.Id == returnedGame.Id, "Game ID in database and API response should match.");
        Assert.True(gameInDb.Prizepool == startingPrizePool, "Prize pool in database and input should match.");
        Assert.True(gameInDb.Date == returnedGame.Date, "Game date in database and API response should match.");
        Assert.True(returnedGame.Status == GameStatus.Active, "Game status should be 'Active' in API response.");
    }
    
    [Fact]
    public async Task Create_Game_API_Test_Creates_Game_Fail_Unauthorized()
    {
        
        var startingPrizePool = 1000;

        var client = CreateClient();

        var response = await client.PostAsJsonAsync("/Game/NewGame", startingPrizePool);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("\"Authentication\":[\"Missing authentication token.\"]", responseContent);
    }
    
    [Fact]
    public async Task Create_Game_API_Test_Fails_When_Prizepool_Is_Negative()
    {
        // Arrange
        var negativePrizePool = -10; // Invalid prize pool
        var guid = new Guid();
        var mockAuthService = new Mock<IAuthService>();
        mockAuthService.Setup(s => s.IsUserAuthenticated(It.IsAny<string>())).Verifiable();
        mockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
            .Returns(new AuthorizedUserResponseDTO() { Id = guid, Name = "TestAdmin", Role = "Admin"  });
        mockAuthService.Setup(s => s.IsUserAuthorized(It.IsAny<string[]>(), It.IsAny<string>())).Verifiable();

        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<IAuthService>(_ => mockAuthService.Object);
                });
            });

        var client = appFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");

        // Act
        var response = await client.PostAsJsonAsync("/Game/NewGame", new { Prizepool = negativePrizePool });
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        _output.WriteLine($"Response Status Code: {response.StatusCode}");
        _output.WriteLine($"Response Content: {responseContent}");
    
        // Expecting Bad Request (400) due to validation failure
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest,
            "API should return BadRequest when prize pool is negative.");
    }
}
