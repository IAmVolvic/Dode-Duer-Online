using System.Net.Http.Json;
using System.Text.Json;
using System.Net;
using DataAccess.Contexts;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Program = API.Program;
using PgCtx;
using Service.TransferModels.Responses;
using Xunit;
using Xunit.Abstractions;

namespace ApiIntegrationTests;

public class GameApiTest : WebApplicationFactory<Program>
{
    private readonly PgCtxSetup<GameContext> _pgCtxSetup = new();
    private readonly ITestOutputHelper _output;

    public GameApiTest(ITestOutputHelper output)
    {
        DotNetEnv.Env.Load();
        _pgCtxSetup = new PgCtxSetup<GameContext>();
        Environment.SetEnvironmentVariable("TestDb",_pgCtxSetup._postgres.GetConnectionString());
        _output = output;
    }
    

    [Fact]
    public async Task Create_Game_API_Test_Creates_Game_With_Starting_Prize_Pool()
    {
        
        var startingPrizePool = 1000;

        var client = CreateClient();

        var response = await client.PostAsJsonAsync("/Game/NewGame", startingPrizePool);
        _output.WriteLine($"Response Status Code: {response.StatusCode}");
        _output.WriteLine($"Response Content: {await response.Content.ReadAsStringAsync()}");
        Assert.True(response.StatusCode == HttpStatusCode.OK, _pgCtxSetup._postgres.GetConnectionString());

        var returnedGame = await response.Content.ReadFromJsonAsync<GameResponseDTO>();
        Assert.NotNull(returnedGame);

        var gameInDb = _pgCtxSetup.DbContextInstance.Games.First();
        Assert.NotNull(gameInDb);

        Assert.True(gameInDb.Id == returnedGame.Id, "Game ID in database and API response should match.");
        Assert.True(gameInDb.Prizepool == startingPrizePool, "Prize pool in database and input should match.");
        Assert.True(gameInDb.Date == returnedGame.Date, "Game date in database and API response should match.");
        Assert.True(returnedGame.Status == "Active", "Game status should be 'Active' in API response.");
    }
    
    [Fact]
    public async Task Create_Game_API_Test_Creates_Game_Without_Starting_Prize_Pool()
    {
        
        var startingPrizePool = 0;

        var client = CreateClient();

        var response = await client.PostAsJsonAsync("/Game/NewGame", startingPrizePool);
        _output.WriteLine($"Response Status Code: {response.StatusCode}");
        _output.WriteLine($"Response Content: {await response.Content.ReadAsStringAsync()}");
        Assert.True(response.StatusCode == HttpStatusCode.OK, _pgCtxSetup._postgres.GetConnectionString());

        var returnedGame = await response.Content.ReadFromJsonAsync<GameResponseDTO>();
        Assert.NotNull(returnedGame);

        var gameInDb = _pgCtxSetup.DbContextInstance.Games.First();
        Assert.NotNull(gameInDb);

        Assert.True(gameInDb.Id == returnedGame.Id, "Game ID in database and API response should match.");
        Assert.True(gameInDb.Prizepool == startingPrizePool, "Prize pool in database and input should match.");
        Assert.True(gameInDb.Date == returnedGame.Date, "Game date in database and API response should match.");
        Assert.True(returnedGame.Status == "Active", "Game status should be 'Active' in API response.");
    }

    [Fact]
    public async Task Create_Game_API_Test_Fails_When_Prizepool_Is_Negative()
    {
        // Arrange
        var negativePrizePool = -10; // Invalid prize pool
        var client = CreateClient();

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
