using System.Net;
using System.Net.Http.Json;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using PgCtx;
using Service.Security;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;
using Xunit;
using Program = API.Program;

namespace ProjectTests.TestClasses;

public class User
{
    private readonly PgCtxSetup<UserContext> _pgCtxSetup;
    private readonly IJWTManager _jwtManager = new JWTManager();
    private readonly WebApplicationFactory<Program> _factory;
    
    public User()
    {
        DotNetEnv.Env.Load();
        // Initialize the WebApplicationFactory with your application class (Program.cs or Startup.cs)
        _factory = new WebApplicationFactory<Program>();
        string connectionString = Environment.GetEnvironmentVariable("DefaultConnection")!;
        _pgCtxSetup = new PgCtxSetup<UserContext>(connectionString);
    }
    
    
    [Fact]
    public async Task Signup()
    {
        var createNewUser = new
        {
            name = "John Doe",
            email = "john@doe.com",
            password = "password"
        };
            
        var client = _factory.CreateClient();
            
        // Simulate a request and get response
        var response = await client.PostAsJsonAsync("/User/signup", createNewUser);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"Signup failed: {response.StatusCode}");
        }
            
        // Ensure response code is OK ( 200 )
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
        // Check the content of the response
        var user = await response.Content.ReadFromJsonAsync<UserResponseDTO>();
            
        Assert.NotNull(user);
        Assert.True(Guid.TryParse(user.Id, out Guid userId), "The returned user ID is not a valid GUID.");
            
        Assert.NotNull(user.JWT);
        Assert.True((_jwtManager.IsJWTValid(user.JWT) != null), "The jwt is not valid.");
    }
}