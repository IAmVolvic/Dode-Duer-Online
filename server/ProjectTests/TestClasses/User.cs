using System.Net;
using System.Net.Http.Json;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PgCtx;
using Service.Security;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;
using Xunit;

namespace ProjectTests.TestClasses;

public class User : WebApplicationFactory<Program>
{
    private readonly PgCtxSetup<UserContext> _pgCtxSetup = new();
    private readonly IJWTManager _jwtManager = new JWTManager();
    
    public User()
    {
        Environment.SetEnvironmentVariable("DefaultConnection", _pgCtxSetup._postgres.GetConnectionString());
    }
    
    
    [Fact]
    public async Task Signup()
    {
        var createNewUser = new UserSignupRequestDTO()
        {
            Name = "John Doe",
            Email = "john@doe.com",
            Password = "password"
        };
        
        var client = CreateClient();
        
        // Sim a request and get response
        var response = await client.PostAsJsonAsync("/User/signup", createNewUser);
        
        // Make sure response code is OK ( 200 )
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        // Check the content
        var user = await response.Content.ReadFromJsonAsync<UserResponseDTO>();
        
        Assert.NotNull(user);
        Assert.True(Guid.TryParse(user.Id, out Guid userId), "The returned user ID is not a valid GUID.");
        
        Assert.NotNull(user.JWT);
        Assert.True((_jwtManager.IsJWTValid(user.JWT) != null), "The jwt is not valid.");
    }
}