using System.Net;
using System.Net.Http.Json;
using System.Transactions;
using ApiInterationTests;
using DataAccess.Models;
using DataAccess.Types.Enums;
using Moq;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;
using Xunit;
using Xunit.Abstractions;
using Transaction = DataAccess.Models.Transaction;

namespace ApiIntegrationTests;

public class TransactionApiTest : ApiTestBase
{
    private readonly ITestOutputHelper _output;

    public TransactionApiTest(ITestOutputHelper output)
    {
        _output = output;
    }
    [Fact]
    public async Task PUserDepositReq_Creates_Deposit_Request()
    {
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
        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");
        var request = new DepositRequestDTO()
        {
            TransactionNumber = "woiuahdawodg"
        };
        var response = await client.PostAsJsonAsync("/Transaction/@user/balance/deposit", request);
        var responseString = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseString);
        var responseValue = await response.Content.ReadFromJsonAsync<TransactionResponseDTO>();
        
        Assert.NotNull(responseValue);
        Assert.Equal(responseValue.TransactionNumber, request.TransactionNumber);
        Assert.Equal(responseValue.PhoneNumber, admin.Phonenumber);
        Assert.Equal(responseValue.UserId, admin.Id);
    }
    /*
    JWT Problems
    [Fact]
    public async Task PUserTransactionsReqs_Gets_Users_Transactions()
    {
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
        
        var transactionId = Guid.NewGuid();
        var deposit = new Transaction()
        {
            Id = transactionId,
            Userid = adminId,
            Transactionnumber = "woiuahdawodg",
            Transactionstatus = TransactionStatusA.Approved
        };
        PgCtxSetup.DbContextInstance.Transactions.Add(deposit);
        PgCtxSetup.DbContextInstance.SaveChanges();
        
        
        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");
        
        var response = await client.GetAsync("Transaction/@user/balance/history");
        var responseString = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseString);
        var responseValue = await response.Content.ReadFromJsonAsync<List<TransactionResponseDTO>>();
        var transaction = responseValue[0];
        
        Assert.NotNull(responseValue);
        Assert.Equal(transaction.TransactionNumber, deposit.Transactionnumber);
        Assert.Equal(transaction.PhoneNumber, admin.Phonenumber);
        Assert.Equal(transaction.UserId, admin.Id);
        Assert.Equal(transaction.Id, deposit.Id);
    }
    */
    [Fact]
    public async Task PDepositReqs_Gets_Users_Transactions()
    {
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
        
        var transactionId = Guid.NewGuid();
        var deposit = new Transaction()
        {
            Id = transactionId,
            Userid = adminId,
            Transactionnumber = "woiuahdawodg",
            Transactionstatus = TransactionStatusA.Approved
        };
        PgCtxSetup.DbContextInstance.Transactions.Add(deposit);
        PgCtxSetup.DbContextInstance.SaveChanges();
        
        
        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");
        
        var response = await client.GetAsync("Transaction/@admin/balance/history");
        var responseString = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseString);
        var responseValue = await response.Content.ReadFromJsonAsync<List<TransactionResponseDTO>>();
        var transaction = responseValue[0];
        
        Assert.NotNull(responseValue);
        Assert.Equal(transaction.TransactionNumber, deposit.Transactionnumber);
        Assert.Equal(transaction.PhoneNumber, admin.Phonenumber);
        Assert.Equal(transaction.UserId, admin.Id);
        Assert.Equal(transaction.Id, deposit.Id);
    }
    /*
     DB ISNT UPDATED 
    [Fact]
    public async Task PUseBalance_Updates_Users_Transactions()
    {
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
        
        var transactionId = Guid.NewGuid();
        var deposit = new Transaction()
        {
            Id = transactionId,
            Userid = adminId,
            Transactionnumber = "woiuahdawodg",
            Transactionstatus = TransactionStatusA.Approved
        };
        PgCtxSetup.DbContextInstance.Transactions.Add(deposit);
        PgCtxSetup.DbContextInstance.SaveChanges();

        var adjustment = new BalanceAdjustmentRequestDTO()
        {
            Adjustment = TransactionAdjustment.Deposit,
            Amount = 1000,
            TransactionStatusA = TransactionStatusA.Pending,
            TransactionId = transactionId.ToString()
        };
        
        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");
        
        var response = await client.PatchAsJsonAsync("Transaction/@admin/balance/adjustment",adjustment);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseString);
        await PgCtxSetup.DbContextInstance.SaveChangesAsync();
        var transactionInDb = await PgCtxSetup.DbContextInstance.Transactions.FindAsync(transactionId);
        var userInDb = await PgCtxSetup.DbContextInstance.Users.FindAsync(adminId);
        
        Assert.Equal(transactionInDb.Transactionnumber, deposit.Transactionnumber);
        Assert.Equal(transactionInDb.Transactionstatus, TransactionStatusA.Approved);
        Assert.Equal( adjustment.Amount+admin.Balance, userInDb.Balance);
    }
    */
}