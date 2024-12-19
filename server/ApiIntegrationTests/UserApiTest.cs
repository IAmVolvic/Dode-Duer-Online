using System.Net;
using System.Net.Http.Json;
using ApiInterationTests;
using DataAccess.Models;
using DataAccess.Types.Enums;
using Moq;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;
using Xunit;
using Xunit.Abstractions;

namespace ApiIntegrationTests;

public class UserApiTest : ApiTestBase
{
    private readonly ITestOutputHelper _output;

    public UserApiTest(ITestOutputHelper output)
    {
        _output = output;
    }
    [Fact]
    public async Task GGetUser_Gets_User()
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
        var response = await client.GetAsync("/User/@user");

    
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadFromJsonAsync<AuthorizedUserResponseDTO>();
        
        Assert.NotNull(responseBody);
        Assert.Equal("TestAdmin", responseBody.Name);
        Assert.Equal(UserRole.Admin, responseBody.Role);
        Assert.Equal(UserStatus.Active, responseBody.Status);
        Assert.Equal(UserEnrolled.True, responseBody.Enrolled);
    }
    /*
    // JWT problems
    [Fact]
    public async Task Login_Logins_User()
    {
        var adminId = Guid.NewGuid();
        
        Assert.NotNull(PgCtxSetup.DbContextInstance);

        MockAuthService.Setup(s => s.GetAuthorizedUser(It.IsAny<string>()))
            .Returns(new AuthorizedUserResponseDTO { Id = adminId, Name = "TestAdmin", Role = UserRole.Admin });

        var admin = new User
        {
            Id = adminId,
            Passwordhash = "AQAAAAIAAYagAAAAEAib8X4hWZaYrUKz/fSJq3AgirViGCJLU3nRRkB+iCTM4w+Moks+3NSJJTYgrIvVJA==",
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

        var user = new UserLoginRequestDTO()
        {
            Email = "admin@gmail.com",
            Password = "admin",
        };
        
        var client = TestHttpClient;
        client.DefaultRequestHeaders.Add("Cookie", "Authentication=valid-token");

        var response = await client.PostAsJsonAsync("/User/@user/login", user);
        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent); 
        
        Assert.NotNull(responseContent);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
    }
    // JWT problems
    [Fact]
    public async Task Update_User_Updates_User()
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
        var userUpdate = new UserUpdateRequestDTO()
        {
            Email = "admin@outlook.com",
            Name = "TestAdmin",
            Password = "admin",
            PhoneNumber = "0521314"
        };
        var response = await client.PostAsJsonAsync("/User/@user/Update",userUpdate);
        var responseBody = await response.Content.ReadFromJsonAsync<AuthorizedUserResponseDTO>();
        
        Assert.NotNull(responseBody);
        Assert.Equal(userUpdate.Email, responseBody.Email);
        Assert.Equal(userUpdate.Name, responseBody.Name);
        Assert.Equal(userUpdate.PhoneNumber, responseBody.PhoneNumber);
    }

*/
    [Fact]
    public async Task User_Signup_Signups_User()
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

        var userToSignUp = new UserSignupRequestDTO()
        {
            Email = "user@gmail.com",
            Name = "TestUser",
            PhoneNumber = "21314521"
        };
        
        var response = await client.PostAsJsonAsync("/User/@admin/signup",userToSignUp);
        var responseBody = await response.Content.ReadFromJsonAsync<AuthorizedUserResponseDTO>();
        
        Assert.NotNull(responseBody);
        Assert.Equal(userToSignUp.Email, responseBody.Email);
        Assert.Equal(userToSignUp.Name, responseBody.Name);
        Assert.Equal(userToSignUp.PhoneNumber, responseBody.PhoneNumber);
    }
}