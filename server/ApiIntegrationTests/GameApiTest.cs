using System.Net.Http.Json;
using System.Net;
using ApiInterationTests;
using DataAccess;
using DataAccess.Models;
using DataAccess.Types.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Program = API.Program;
using PgCtx;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;
using Xunit;
using Xunit.Abstractions;

namespace ApiIntegrationTests;

public class GameApiTest : ApiTestBase
{
    [Theory]
    [InlineData(0)]
    [InlineData(1000)]
    public async Task Create_Game_API_Test_Creates_Game_With_Starting_Prize_Pool(int startingPrizePool)
    {
        MockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
            .Returns(new AuthorizedUserResponseDTO() { Id = Guid.NewGuid(), Name = "TestAdmin", Role = UserRole.Admin });

        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");

        var response = await client.PostAsJsonAsync("/Game/NewGame", startingPrizePool);
        Assert.True(response.StatusCode == HttpStatusCode.OK);

        var returnedGame = await response.Content.ReadFromJsonAsync<GameResponseDTO>();
        Assert.NotNull(returnedGame);

        var gameInDb = PgCtxSetup.DbContextInstance.Games.First(g => g.Id == returnedGame.Id);
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

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_Game_API_Test_Fails_When_Prizepool_Is_Negative()
    {
        // Arrange
        var negativePrizePool = -10;
        
        MockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
            .Returns(new AuthorizedUserResponseDTO() { Id = Guid.NewGuid(), Name = "TestAdmin", Role = UserRole.Admin });

        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");

        // Act
        var response = await client.PostAsJsonAsync("/Game/NewGame", new { Prizepool = negativePrizePool });

        // Expecting Bad Request (400) due to validation failure
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest,
            "API should return BadRequest when prize pool is negative.");
    }

    [Fact]
    public async Task Set_Winning_Numbers_Sets_Winning_Numbers()
    {
        var adminId = Guid.NewGuid();
        
        Assert.NotNull(PgCtxSetup.DbContextInstance);
        MockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
            .Returns(new AuthorizedUserResponseDTO() { Id = adminId, Name = "TestAdmin", Role = UserRole.Admin });
        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");

        var response = await client.PostAsJsonAsync("/Game/NewGame", 0);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
        
        var returnedGame = await response.Content.ReadFromJsonAsync<GameResponseDTO>();
        Assert.NotNull(returnedGame);

        var gameId = returnedGame.Id;

        var winningNumbers = new WinningNumbersRequestDTO()
        {
            GameId = gameId,
            WinningNumbers = [1, 2, 3]
        };
        var winningNumbersResponse = await client.PostAsJsonAsync("/Game/winningNumbers", winningNumbers);
        
        var returnedNumbers = await winningNumbersResponse.Content.ReadFromJsonAsync<WinningNumbersResponseDTO>();
        
        Assert.NotNull(returnedNumbers);
        Assert.Equal(returnedNumbers.Gameid,gameId);
        Assert.Equal(returnedNumbers.Winningnumbers.OrderBy(x => x), winningNumbers.WinningNumbers.OrderBy(x => x));
    }
}
