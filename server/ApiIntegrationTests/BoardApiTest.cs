using System.Net;
using DataAccess.Models;
using Service.TransferModels.Requests;
using Xunit;
using Xunit.Abstractions;
using System.Net.Http.Json;
using ApiInterationTests;
using Microsoft.EntityFrameworkCore;
using Moq;
using Service.TransferModels.Responses;

namespace ApiIntegrationTests;

public class BoardApiTest : ApiTestBase
{
    private readonly ITestOutputHelper _output;

    public BoardApiTest(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
public async Task Play_Board_Works_When_All_Parameters_Are_Valid()
{
    
    // Assert the DbContextInstance is initialized
    Assert.NotNull(PgCtxSetup.DbContextInstance);

    var guidUser = Guid.NewGuid();
    MockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
        .Returns(new AuthorizedUserResponseDTO { Id = guidUser, Name = "John Doe", Role = "User" });

    await PgCtxSetup.DbContextInstance.Database.ExecuteSqlRawAsync(@"
INSERT INTO prices (id, price, numbers)
VALUES
    ('95f9a200-4538-4e43-8674-38b67579b8a7', 20.00, 5.00),
    ('1cd3f690-2eeb-405c-b8a7-922c80f0cb3d', 40.00, 6.00),
    ('af8c5461-d9ec-4ede-9bf0-abf2ffac9895', 80.00, 7.00),
    ('ac6f719f-e9e0-4fde-9b31-a12562f7ce01', 160.00, 8.00);
"); 
    PgCtxSetup.DbContextInstance.SaveChanges();
    var player = new User()
    {
        Id = guidUser,
        Name = "John Doe",
        Email = "john.doe@gmail.com",
        Passwordhash = "wipadjawpodjiawdpawidjpaw",
        Phonenumber = "01234561",
        Balance = 1000
    };

    // Add user and verify it's saved
    PgCtxSetup.DbContextInstance.Users.Add(player);
    PgCtxSetup.DbContextInstance.SaveChanges();
    Assert.NotNull(PgCtxSetup.DbContextInstance.Users.Find(guidUser));

    var board = new PlayBoardDTO()
    {
        Dateofpurchase = DateOnly.FromDateTime(DateTime.Now),
        Numbers = new List<int> { 1, 2, 3, 4, 5 },
        Userid = guidUser
    };

    var response = await TestHttpClient.PostAsJsonAsync("/Board/Play", board);
    Assert.NotNull(response);
    var responseBody = await response.Content.ReadAsStringAsync();
    _output.WriteLine($"Response Status: {response.StatusCode}");
    _output.WriteLine($"Response Body: {responseBody}");
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var returnedBoard = await response.Content.ReadFromJsonAsync<BoardResponseDTO>();
    Assert.NotNull(returnedBoard);

    var boardInDb = PgCtxSetup.DbContextInstance.Boards.FirstOrDefault();
    Assert.NotNull(boardInDb);

    var chosenNumbersInDb = PgCtxSetup.DbContextInstance.Chosennumbers.ToList();
    Assert.NotEmpty(chosenNumbersInDb);

    var numbersInDb = chosenNumbersInDb.Select(n => n.Number).ToList();
    Assert.NotEmpty(numbersInDb);

    Assert.Equal(boardInDb.Userid, guidUser);
    Assert.Equal(boardInDb.Userid, returnedBoard.Userid);
    Assert.Equal(boardInDb.Gameid, returnedBoard.Gameid);
    Assert.Equal(boardInDb.Dateofpurchase, returnedBoard.Dateofpurchase);
    Assert.Equal(numbersInDb.OrderBy(x => x), returnedBoard.Numbers.OrderBy(x => x));
}


    [Theory]
    [InlineData(new int[] { 1, 2, 3 })]
    [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })]
    public async Task Play_Board_Doesnt_Work_When_There_Is_Unsufficient_Amount_Of_Numbers(int[] numbers)
    {
        // Arrange
        var guidUser = Guid.NewGuid();
        MockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
            .Returns(new AuthorizedUserResponseDTO { Id = guidUser, Name = "John Doe", Role = "User" });

        var player = new User
        {
            Id = guidUser,
            Name = "John Doe",
            Email = "john.doe@gmail.com",
            Passwordhash = "hashedpassword",
            Phonenumber = "01234561",
            Balance = 1000
        };

        PgCtxSetup.DbContextInstance.Users.Add(player);
        PgCtxSetup.DbContextInstance.SaveChanges();

        var board = new PlayBoardDTO
        {
            Dateofpurchase = DateOnly.FromDateTime(DateTime.Now),
            Numbers = numbers.ToList(),
            Userid = guidUser
        };

        var client = TestHttpClient;

        // Act
        var response = await client.PostAsJsonAsync("/Board/Play", board);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var boardInDb = PgCtxSetup.DbContextInstance.Boards.FirstOrDefault();
        Assert.Null(boardInDb);
    }

    [Theory]
    [InlineData(new int[] { 0, 2, 3, 5, 6 })]
    [InlineData(new int[] { 1, 2, 3, 4, 17 })]
    public async Task Play_Board_Doesnt_Work_When_Numbers_Arent_Within_Range(int[] numbers)
    {
        // Arrange
        var guidUser = Guid.NewGuid();
        MockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
            .Returns(new AuthorizedUserResponseDTO { Id = guidUser, Name = "John Doe", Role = "User" });

        var player = new User
        {
            Id = guidUser,
            Name = "John Doe",
            Email = "john.doe@gmail.com",
            Passwordhash = "hashedpassword",
            Phonenumber = "01234561",
            Balance = 1000
        };

        PgCtxSetup.DbContextInstance.Users.Add(player);
        PgCtxSetup.DbContextInstance.SaveChanges();

        var board = new PlayBoardDTO
        {
            Dateofpurchase = DateOnly.FromDateTime(DateTime.Now),
            Numbers = numbers.ToList(),
            Userid = guidUser
        };

        var client = TestHttpClient;

        // Act
        var response = await client.PostAsJsonAsync("/Board/Play", board);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var boardInDb = PgCtxSetup.DbContextInstance.Boards.FirstOrDefault();
        Assert.Null(boardInDb);
    }
}
