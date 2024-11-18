using API.Exceptions;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Service.Security;
using Service.Services.Interfaces;
using Service.TransferModels.Responses;

namespace Service.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IJWTManager _jwtManager;
    
    public AuthService(IPasswordHasher<User> passwordHasher, IJWTManager jwtManager, IUserRepository userRepository)
    {
        _repository = userRepository;
        _jwtManager = jwtManager;
    }
    
    
    public AuthorizedUserResponseDTO GetAuthorizedUser(string jwtToken)
    {
        var jwtData = _jwtManager.IsJWTValid(jwtToken);
        var uuidClaim = jwtData.Claims.FirstOrDefault(claim => claim.Type == "uuid");
        var userData = _repository.GetUserById(uuidClaim.Value);
        
        if (userData == null)
        {
            throw new ErrorException("User", "User does not exist");
        }
        
        return AuthorizedUserResponseDTO.FromEntity(userData);
    }
    

    public void IsUserAuthenticated(string jwtToken)
    {
        var jwtData = _jwtManager.IsJWTValid(jwtToken);
    
        if (jwtData == null)
        {
            throw new ErrorException("Authentication", "Authentication failed due to invalid token");
        }
    }

    
    public void IsUserAuthorized(string[] roles, string jwtToken)
    {
        var userData = GetAuthorizedUser(jwtToken);
        
        if (!roles.Contains(userData.Role))
        {
            throw new ErrorException("Authorization", "User does not have the required role.");
        }
    }
}
