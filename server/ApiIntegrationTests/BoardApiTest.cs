using System.Net;
using API;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using PgCtx;
using Service.TransferModels.Requests;
using Xunit;
using Xunit.Abstractions;
using System.Net.Http.Json;
using Castle.Components.DictionaryAdapter.Xml;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Service.TransferModels.Responses;

namespace ApiIntegrationTests;

public class BoardApiTest: WebApplicationFactory<Program>
{
    private readonly PgCtxSetup<LotteryContext> _pgCtxSetupUser = new();
    private readonly ITestOutputHelper _output;

    public BoardApiTest(ITestOutputHelper output)
    {
        DotNetEnv.Env.Load("../API/.env");
        _pgCtxSetupUser = new PgCtxSetup<LotteryContext>();
        Environment.SetEnvironmentVariable("TestDb",_pgCtxSetupUser._postgres.GetConnectionString());
        _output = output;
    }

    [Fact]
    public async Task Play_Board_Works_When_All_Parameters_Are_Valid()
    {
        var guidUser = Guid.NewGuid();
        var player = new User()
        {
            Id = guidUser,
            Name = "John Doe",
            Email = "john.doe@gmail.com",
            Passwordhash = "wipadjawpodjiawdpawidjpaw",
            Phonenumber = "01234561",
        };
        
        _pgCtxSetupUser.DbContextInstance.Users.Add(player);
        _pgCtxSetupUser.DbContextInstance.SaveChanges();
        
        var guidGame = Guid.NewGuid();
        var game = new Game()
        {
            Id = guidGame,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Prizepool = 0
        };
        
        _pgCtxSetupUser.DbContextInstance.Games.Add(game);
        _pgCtxSetupUser.DbContextInstance.SaveChanges();

        var board = new PlayBoardDTO()
        {
            Dateofpurchase = DateOnly.FromDateTime(DateTime.Now),
            Numbers = [1, 2, 4, 5, 7],
            Userid = guidUser
        };
        
        _pgCtxSetupUser.DbContextInstance.Database.ExecuteSqlRaw(@"
INSERT INTO prices (id, price, numbers)
VALUES
    ('95f9a200-4538-4e43-8674-38b67579b8a7', 20.00, 5.00),
    ('1cd3f690-2eeb-405c-b8a7-922c80f0cb3d', 40.00, 6.00),
    ('af8c5461-d9ec-4ede-9bf0-abf2ffac9895', 80.00, 7.00),
    ('ac6f719f-e9e0-4fde-9b31-a12562f7ce01', 160.00, 8.00);");

        var client = CreateClient();

        var response = await client.PostAsJsonAsync("/Board/Play", board );
        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var returnedBoard = await response.Content.ReadFromJsonAsync<BoardResponseDTO>();
        Assert.NotNull(returnedBoard);
        var boardInDb = _pgCtxSetupUser.DbContextInstance.Boards.First();
        Assert.NotNull(boardInDb);
        var chosenNumbersInDb = _pgCtxSetupUser.DbContextInstance.Chosennumbers.ToList();
        Assert.NotNull(chosenNumbersInDb);
        var numbersInDb = chosenNumbersInDb.Select(n => n.Number).ToList();
        Assert.NotNull(numbersInDb);
        
        Assert.Equal(boardInDb.Userid, guidUser);
        Assert.Equal(boardInDb.Userid, returnedBoard.Userid);
        Assert.Equal(boardInDb.Gameid, guidGame);
        Assert.Equal(boardInDb.Gameid, returnedBoard.Gameid);
        Assert.Equal(boardInDb.Dateofpurchase, returnedBoard.Dateofpurchase);
        Assert.Equal(boardInDb.Dateofpurchase, DateOnly.FromDateTime(DateTime.Now));
        Assert.Equal(numbersInDb.OrderBy(x => x), returnedBoard.Numbers.OrderBy(x => x));
    }
    
    [Theory]
    [InlineData(new int[] { 1, 2, 3 })]
    [InlineData(new int[] { 1, 2, 3,4,5,6,7,8,9 })]
    public async Task Play_Board_Doesnt_Work_When_There_Is_Unsaficient_Amount_Of_Numbers(int[] numbers)
    {
        var numbersList = numbers.ToList();
        var guidUser = Guid.NewGuid();
        var player = new User()
        {
            Id = guidUser,
            Name = "John Doe",
            Email = "john.doe@gmail.com",
            Passwordhash = "wipadjawpodjiawdpawidjpaw",
            Phonenumber = "01234561",
        };
        
        _pgCtxSetupUser.DbContextInstance.Users.Add(player);
        _pgCtxSetupUser.DbContextInstance.SaveChanges();
        
        var guidGame = Guid.NewGuid();
        var game = new Game()
        {
            Id = guidGame,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Prizepool = 0
        };
        
        _pgCtxSetupUser.DbContextInstance.Games.Add(game);
        _pgCtxSetupUser.DbContextInstance.SaveChanges();

        var board = new PlayBoardDTO()
        {
            Dateofpurchase = DateOnly.FromDateTime(DateTime.Now),
            Numbers = numbersList,
            Userid = guidUser
        };
        
        _pgCtxSetupUser.DbContextInstance.Database.ExecuteSqlRaw(@"
INSERT INTO prices (id, price, numbers)
VALUES
    ('95f9a200-4538-4e43-8674-38b67579b8a7', 20.00, 5.00),
    ('1cd3f690-2eeb-405c-b8a7-922c80f0cb3d', 40.00, 6.00),
    ('af8c5461-d9ec-4ede-9bf0-abf2ffac9895', 80.00, 7.00),
    ('ac6f719f-e9e0-4fde-9b31-a12562f7ce01', 160.00, 8.00);");

        var client = CreateClient();

        var response = await client.PostAsJsonAsync("/Board/Play", board );
        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var boardInDb = _pgCtxSetupUser.DbContextInstance.Boards.FirstOrDefault();
        Assert.Null(boardInDb);
    }
    
    [Theory]
    [InlineData(new int[] { 0, 2, 3,5,6 })]
    [InlineData(new int[] { 1, 2, 3,4,17})]
    public async Task Play_Board_Doesnt_Work_When_Numbers_Arent_Within_Range(int[] numbers)
    {
        var numbersList = numbers.ToList();
        var guidUser = Guid.NewGuid();
        var player = new User()
        {
            Id = guidUser,
            Name = "John Doe",
            Email = "john.doe@gmail.com",
            Passwordhash = "wipadjawpodjiawdpawidjpaw",
            Phonenumber = "01234561",
        };
        
        _pgCtxSetupUser.DbContextInstance.Users.Add(player);
        _pgCtxSetupUser.DbContextInstance.SaveChanges();
        
        var guidGame = Guid.NewGuid();
        var game = new Game()
        {
            Id = guidGame,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Prizepool = 0
        };
        
        _pgCtxSetupUser.DbContextInstance.Games.Add(game);
        _pgCtxSetupUser.DbContextInstance.SaveChanges();

        var board = new PlayBoardDTO()
        {
            Dateofpurchase = DateOnly.FromDateTime(DateTime.Now),
            Numbers = numbersList,
            Userid = guidUser
        };
        
        _pgCtxSetupUser.DbContextInstance.Database.ExecuteSqlRaw(@"
INSERT INTO prices (id, price, numbers)
VALUES
    ('95f9a200-4538-4e43-8674-38b67579b8a7', 20.00, 5.00),
    ('1cd3f690-2eeb-405c-b8a7-922c80f0cb3d', 40.00, 6.00),
    ('af8c5461-d9ec-4ede-9bf0-abf2ffac9895', 80.00, 7.00),
    ('ac6f719f-e9e0-4fde-9b31-a12562f7ce01', 160.00, 8.00);");

        var client = CreateClient();

        var response = await client.PostAsJsonAsync("/Board/Play", board );
        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var boardInDb = _pgCtxSetupUser.DbContextInstance.Boards.FirstOrDefault();
        Assert.Null(boardInDb);
    }
}