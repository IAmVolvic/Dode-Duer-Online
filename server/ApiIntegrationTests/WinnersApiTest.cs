using System.Net;
using System.Net.Http.Json;
using ApiInterationTests;
using DataAccess.Models;
using DataAccess.Types.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;
using Service.TransferModels.Responses;
using Xunit;
using Xunit.Abstractions;

namespace ApiIntegrationTests;

public class WinnersApiTest: ApiTestBase
{
    private readonly ITestOutputHelper _output;

    public WinnersApiTest(ITestOutputHelper output)
    {
        _output = output;
    }
    [Fact]
    public async Task Getting_Winners_Gets_Correct_Data()
    {
        var adminId = Guid.NewGuid();
        
        Assert.NotNull(PgCtxSetup.DbContextInstance);
        MockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
            .Returns(new AuthorizedUserResponseDTO() { Id = adminId, Name = "TestAdmin", Role = UserRole.Admin });
        var admin = new User()
        {
            Id = adminId,
            Passwordhash = "waoudahowdwahduaoiwd",
            Phonenumber = "01234561",
            Balance = 1000,
            Name = "TestAdmin",
            Role = UserRole.Admin,
            Email = "admin@gmail.com",
            Status = UserStatus.Active,
            Enrolled = UserEnrolled.True
        };
        PgCtxSetup.DbContextInstance.Users.Add(admin);
        PgCtxSetup.DbContextInstance.SaveChanges();
        
        await PgCtxSetup.DbContextInstance.Database.ExecuteSqlRawAsync(@"
    INSERT INTO prices (id, price, numbers)
    VALUES
        ('95f9a200-4538-4e43-8674-38b67579b8a7', 20.00, 5.00),
        ('1cd3f690-2eeb-405c-b8a7-922c80f0cb3d', 40.00, 6.00),
        ('af8c5461-d9ec-4ede-9bf0-abf2ffac9895', 80.00, 7.00),
        ('ac6f719f-e9e0-4fde-9b31-a12562f7ce01', 160.00, 8.00);
    ");
        
        PgCtxSetup.DbContextInstance.SaveChanges();
        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");
        
        var gameResponse = await client.PostAsJsonAsync("/Game/NewGame", 0);
        var gameResponseContent = await gameResponse.Content.ReadFromJsonAsync<GameResponseDTO>();
        var gameId = gameResponseContent.Id;

        var boardId = Guid.NewGuid();
        var board = new Board()
        {
            Id = boardId,
            Userid = adminId,
            Gameid = gameId,
            Dateofpurchase = DateOnly.FromDateTime(DateTime.Now),
            Priceid = Guid.Parse("95f9a200-4538-4e43-8674-38b67579b8a7"),
            Chosennumbers =
            [
                new Chosennumber() { Id = Guid.NewGuid(), Boardid = boardId, Number = 1 },
                new Chosennumber() { Id = Guid.NewGuid(), Boardid = boardId, Number = 5 },
                new Chosennumber() { Id = Guid.NewGuid(), Boardid = boardId, Number = 4 },
                new Chosennumber() { Id = Guid.NewGuid(), Boardid = boardId, Number = 3 },
                new Chosennumber() { Id = Guid.NewGuid(), Boardid = boardId, Number = 2 },
            ]
        };
        PgCtxSetup.DbContextInstance.Boards.Add(board);
        PgCtxSetup.DbContextInstance.SaveChanges();
        var winningNumbers = new List<WinningNumbers>
        {
            new WinningNumbers() { GameId = gameId, Number = 1, Id = Guid.NewGuid() },
            new WinningNumbers() { GameId = gameId, Number = 2, Id = Guid.NewGuid() },
            new WinningNumbers() { GameId = gameId, Number = 3, Id = Guid.NewGuid() },
        };
        PgCtxSetup.DbContextInstance.WinningNumbers.AddRange(winningNumbers);
        PgCtxSetup.DbContextInstance.SaveChanges();
        var winner = new Winner()
        {
            Id = Guid.NewGuid(),
            Userid = adminId,
            Gameid = gameId,
            Wonamount = 50
        };
        PgCtxSetup.DbContextInstance.Winners.Add(winner);
        PgCtxSetup.DbContextInstance.SaveChanges();

        var response = await client.GetAsync($"Winners/getWinners/{gameId}");
        _output.WriteLine($"Response Status Code: {response.StatusCode}");
        _output.WriteLine($"Response Content: {await response.Content.ReadAsStringAsync()}");
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var winnersResponse = await response.Content.ReadFromJsonAsync<List<WinnersDto>>();
        Assert.NotNull(winnersResponse);
        Assert.Single(winnersResponse);

        // Assert - Validate winner data
        var returnedWinner = winnersResponse.First();
        Assert.Equal(winner.Userid, returnedWinner.UserId);
        Assert.Equal(winner.Gameid, returnedWinner.Gameid);
        Assert.Equal(winner.Wonamount, returnedWinner.Prize);
    }

    [Fact]
    public async Task Establishing_Winners_Works_As_Expected()
    {
         var adminId = Guid.NewGuid();
        
        Assert.NotNull(PgCtxSetup.DbContextInstance);
        MockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
            .Returns(new AuthorizedUserResponseDTO() { Id = adminId, Name = "TestAdmin", Role = UserRole.Admin });
        var admin = new User()
        {
            Id = adminId,
            Passwordhash = "waoudahowdwahduaoiwd",
            Phonenumber = "01234561",
            Balance = 1000,
            Name = "TestAdmin",
            Role = UserRole.Admin,
            Email = "admin@gmail.com",
            Status = UserStatus.Active,
            Enrolled = UserEnrolled.True
        };
        PgCtxSetup.DbContextInstance.Users.Add(admin);
        PgCtxSetup.DbContextInstance.SaveChanges();
        
        await PgCtxSetup.DbContextInstance.Database.ExecuteSqlRawAsync(@"
    INSERT INTO prices (id, price, numbers)
    VALUES
        ('95f9a200-4538-4e43-8674-38b67579b8a7', 20.00, 5.00),
        ('1cd3f690-2eeb-405c-b8a7-922c80f0cb3d', 40.00, 6.00),
        ('af8c5461-d9ec-4ede-9bf0-abf2ffac9895', 80.00, 7.00),
        ('ac6f719f-e9e0-4fde-9b31-a12562f7ce01', 160.00, 8.00);
    ");
        
        PgCtxSetup.DbContextInstance.SaveChanges();
        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");
        
        var gameResponse = await client.PostAsJsonAsync("/Game/NewGame", 0);
        var gameResponseContent = await gameResponse.Content.ReadFromJsonAsync<GameResponseDTO>();
        var gameId = gameResponseContent.Id;

        var boardId = Guid.NewGuid();
        var board = new Board()
        {
            Id = boardId,
            Userid = adminId,
            Gameid = gameId,
            Dateofpurchase = DateOnly.FromDateTime(DateTime.Now),
            Priceid = Guid.Parse("95f9a200-4538-4e43-8674-38b67579b8a7"),
            Chosennumbers =
            [
                new Chosennumber() { Id = Guid.NewGuid(), Boardid = boardId, Number = 1 },
                new Chosennumber() { Id = Guid.NewGuid(), Boardid = boardId, Number = 5 },
                new Chosennumber() { Id = Guid.NewGuid(), Boardid = boardId, Number = 4 },
                new Chosennumber() { Id = Guid.NewGuid(), Boardid = boardId, Number = 3 },
                new Chosennumber() { Id = Guid.NewGuid(), Boardid = boardId, Number = 2 },
            ]
        };
        PgCtxSetup.DbContextInstance.Boards.Add(board);
        PgCtxSetup.DbContextInstance.SaveChanges();
        var winningNumbers = new List<WinningNumbers>
        {
            new WinningNumbers() { GameId = gameId, Number = 1, Id = Guid.NewGuid() },
            new WinningNumbers() { GameId = gameId, Number = 2, Id = Guid.NewGuid() },
            new WinningNumbers() { GameId = gameId, Number = 3, Id = Guid.NewGuid() },
        };
        PgCtxSetup.DbContextInstance.WinningNumbers.AddRange(winningNumbers);
        PgCtxSetup.DbContextInstance.SaveChanges();
        
        var response = await client.GetAsync($"/Winners/establishWinners/{gameId}");
        var winnersContent = await response.Content.ReadFromJsonAsync<List<WinnersDto>>();
        
        Assert.NotNull(winnersContent);
        Assert.Single(winnersContent);

        // Assert - Validate winner data
        var returnedWinner = winnersContent.First();
        Assert.Equal(admin.Id, returnedWinner.UserId);
        Assert.Equal(gameId, returnedWinner.Gameid);
        Assert.Equal(gameResponseContent.Prize*0.7m, returnedWinner.Prize);
    }
}