using System.Net;
using DataAccess.Models;
using Service.TransferModels.Requests;
using Xunit;
using Xunit.Abstractions;
using System.Net.Http.Json;
using ApiInterationTests;
using DataAccess.Types.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;
using Org.BouncyCastle.Crypto.Engines;
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
        .Returns(new AuthorizedUserResponseDTO { Id = guidUser, Name = "John Doe", Role = UserRole.User });

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
            .Returns(new AuthorizedUserResponseDTO { Id = guidUser, Name = "John Doe", Role = UserRole.User });

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
            .Returns(new AuthorizedUserResponseDTO { Id = guidUser, Name = "John Doe", Role = UserRole.User });

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

   [Fact]
    public async Task Autoplay_Plays_Autoplay_Board()
    {
        // Assert the DbContextInstance is initialized
        Assert.NotNull(PgCtxSetup.DbContextInstance);

        var guidUser = Guid.NewGuid();
        MockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
            .Returns(new AuthorizedUserResponseDTO { Id = guidUser, Name = "John Doe", Role = UserRole.User });

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

        var autoplay = new PlayAutoplayBoardDTO()
        {
            Userid = guidUser,
            LeftToPlay = 10,
            Numbers = new List<int> { 1, 2, 3, 4, 5 }
        };

        var client = TestHttpClient;

        var response = await client.PostAsJsonAsync("/Board/Autoplay", autoplay);
        Assert.NotNull(response);
        var responseBody = await response.Content.ReadAsStringAsync();
        _output.WriteLine($"Response Status: {response.StatusCode}");
        _output.WriteLine($"Response Body: {responseBody}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var boardInDb = PgCtxSetup.DbContextInstance.BoardAutoplays.FirstOrDefault();
        Assert.NotNull(boardInDb);
        Assert.Equal(guidUser, boardInDb.UserId);

        var chosenNumbersInDb = PgCtxSetup.DbContextInstance.ChosenNumbersAutoplays.ToList();
        Assert.NotEmpty(chosenNumbersInDb);

        var numbersInDb = chosenNumbersInDb.Select(n => n.Number).ToList();
        Assert.NotEmpty(numbersInDb);
        
        Assert.Equal(autoplay.Numbers.OrderBy(x => x), numbersInDb.OrderBy(x => x));

        // Ensure the response contains the expected board data
        var returnedBoard = await response.Content.ReadFromJsonAsync<AutoplayBoardDTO>();
        Assert.NotNull(returnedBoard);
        Assert.Equal(boardInDb.UserId, returnedBoard.Userid);
        Assert.Equal(boardInDb.LeftToPlay, returnedBoard.LeftToPlay);
        Assert.Equal(boardInDb.Id, returnedBoard.Id);
        Assert.Equal(numbersInDb.OrderBy(x => x), returnedBoard.Numbers.OrderBy(x => x));
    }

    [Fact]
    public async Task Get_All_Boards_Gets_All_Boards()
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
        
        var board = new PlayBoardDTO()
        {
            Dateofpurchase = DateOnly.FromDateTime(DateTime.Now),
            Numbers = new List<int> { 1, 2, 3, 4, 5 },
            Userid = adminId
        };
        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");

        // Act
        var boardResponse = await client.PostAsJsonAsync("/Board/Play", board);
        Assert.NotNull(boardResponse);
        Assert.Equal(HttpStatusCode.OK, boardResponse.StatusCode);

        var response = await client.GetAsync("/Board/GetBoards");
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        _output.WriteLine($"Response Status: {response.StatusCode}");
        _output.WriteLine($"Response Body: {responseBody}");

        var boards = await response.Content.ReadFromJsonAsync<List<BoardResponseDTO>>();
        Assert.NotNull(boards);
        Assert.NotEmpty(boards);

        // Verify the board created earlier is present in the response
        var createdBoard = boards.FirstOrDefault();
        Assert.NotNull(createdBoard);
        Assert.Equal(adminId, createdBoard.Userid);
        Assert.Equal(
            board.Numbers.OrderBy(x => x), 
            createdBoard.Numbers.Where(n => n.HasValue).Select(n => n.Value).OrderBy(x => x)
        );
        Assert.Equal(board.Dateofpurchase, createdBoard.Dateofpurchase);

        // Ensure the database contains at least one board
        var boardInDb = PgCtxSetup.DbContextInstance.Boards.FirstOrDefault();
        Assert.NotNull(boardInDb);

        // Verify the board saved in the database matches the response
        Assert.Equal(createdBoard.Gameid, boardInDb.Gameid);
        Assert.Equal(createdBoard.Userid, boardInDb.Userid);
        Assert.Equal(createdBoard.Dateofpurchase, boardInDb.Dateofpurchase);
    }
    
    [Fact]
public async Task Get_Boards_From_Game_Returns_Correct_Boards()
{
    // Arrange
    var adminId = Guid.NewGuid();
    

    Assert.NotNull(PgCtxSetup.DbContextInstance);

    MockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
        .Returns(new AuthorizedUserResponseDTO { Id = adminId, Name = "TestAdmin", Role = UserRole.Admin });

    var admin = new User
    {
        Id = adminId,
        Passwordhash = "adminpasswordhash",
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
    
    var board = new PlayBoardDTO()
    {
        Dateofpurchase = DateOnly.FromDateTime(DateTime.Now),
        Numbers = new List<int> { 1, 2, 3, 4, 5 },
        Userid = adminId
    };
    var boardResponse = await client.PostAsJsonAsync("/Board/Play", board);
    
    
    var response = await client.GetAsync($"/Board/GetBoardsFromGame/{gameId}");

    
    Assert.NotNull(response);
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var responseBody = await response.Content.ReadAsStringAsync();
    _output.WriteLine($"Response Status: {response.StatusCode}");
    _output.WriteLine($"Response Body: {responseBody}");

    var boards = await response.Content.ReadFromJsonAsync<List<BoardGameResponseDTO>>();
    Assert.NotNull(boards);
    Assert.NotEmpty(boards);

    var retrievedBoard = boards.FirstOrDefault();
    Assert.NotNull(retrievedBoard);
    Assert.Equal(adminId, retrievedBoard.userId);
    Assert.Equal(board.Dateofpurchase, retrievedBoard.Dateofpurchase);
    Assert.Equal(
        board.Numbers.OrderBy(x => x),
        retrievedBoard.Numbers.Where(n => n.HasValue).Select(n => n.Value).OrderBy(x => x)
    );
}

    [Fact]
    public async Task Get_Autoplay_Boards_Returns_Correct_Boards()
    {
        Assert.NotNull(PgCtxSetup.DbContextInstance);

        var guidUser = Guid.NewGuid();
        MockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
            .Returns(new AuthorizedUserResponseDTO { Id = guidUser, Name = "John Doe", Role = UserRole.User });

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

        PgCtxSetup.DbContextInstance.Users.Add(player);
        PgCtxSetup.DbContextInstance.SaveChanges();
        Assert.NotNull(PgCtxSetup.DbContextInstance.Users.Find(guidUser));
        var boardId = Guid.NewGuid();
        var autoplay = new BoardAutoplay()
        {
            Id = boardId,
            UserId = guidUser,
            LeftToPlay = 10,
            ChosenNumbersAutoplays = new List<ChosenNumbersAutoplay>
            {
                new ChosenNumbersAutoplay() {Id = Guid.NewGuid(), BoardId = boardId,Number = 1},
                new ChosenNumbersAutoplay() {Id = Guid.NewGuid(), BoardId = boardId,Number = 2},
                new ChosenNumbersAutoplay() {Id = Guid.NewGuid(), BoardId = boardId,Number = 3},
                new ChosenNumbersAutoplay() {Id = Guid.NewGuid(), BoardId = boardId,Number = 4},
                new ChosenNumbersAutoplay() {Id = Guid.NewGuid(), BoardId = boardId,Number = 5},
            }
        };
        PgCtxSetup.DbContextInstance.BoardAutoplays.Add(autoplay);
        PgCtxSetup.DbContextInstance.SaveChanges();
        
        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");

        // Act
        var response = await client.GetAsync($"/Board/GetAutoplayBoards/{guidUser}");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var autoplayBoards = await response.Content.ReadFromJsonAsync<List<AutoplayBoardDTO>>();
        Assert.NotNull(autoplayBoards);
        Assert.Single(autoplayBoards);

        var retrievedBoard = autoplayBoards.First();
        Assert.Equal(boardId, retrievedBoard.Id);
        Assert.Equal(guidUser, retrievedBoard.Userid);
        Assert.Equal(10, retrievedBoard.LeftToPlay);
        Assert.Equal(
            new List<int> { 1, 2, 3, 4, 5 }.OrderBy(x => x),
            retrievedBoard.Numbers.OrderBy(x => x)
        );
    }

    [Fact]
    public async Task Get_My_Boards_Returns_Correct_Boards()
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
        
        var board = new PlayBoardDTO()
        {
            Dateofpurchase = DateOnly.FromDateTime(DateTime.Now),
            Numbers = new List<int> { 1, 2, 3, 4, 5 },
            Userid = adminId
        };
        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");

        // Act
        var boardResponse = await client.PostAsJsonAsync("/Board/Play", board);
        Assert.NotNull(boardResponse);
        Assert.Equal(HttpStatusCode.OK, boardResponse.StatusCode);

        var historyResponse = await client.GetAsync("/Board/@me/History");
        Assert.NotNull(historyResponse);
        Assert.Equal(HttpStatusCode.OK, historyResponse.StatusCode);

        var history = await historyResponse.Content.ReadFromJsonAsync<List<MyBoards>>();
        Assert.NotNull(history);
        Assert.Single(history);

        // Assert the returned board history
        var myBoards = history.First();
        Assert.NotNull(myBoards);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), myBoards.Boards.First().DateOfPurchase);

        var returnedNumbers = myBoards.Boards.First().Numbers.OrderBy(x => x).ToList();
        Assert.NotNull(returnedNumbers);
        Assert.Equal(new List<int?> { 1, 2, 3, 4, 5 }, returnedNumbers);
        Assert.Equal(0, myBoards.Boards.First().WinningAmount); // Assuming no winnings at this stage
    }
}
