using API.Exceptions;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Service.Security;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services;

public class UserService : IUserService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _repository;
    private readonly IJWTManager _jwtManager;
    
    public UserService(IPasswordHasher<User> passwordHasher, IJWTManager jwtManager, IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _repository = userRepository;
        _jwtManager = jwtManager;
    }

    public UserResponseDTO Signup(UserSignupRequestDTO newUser)
    {
        Guid userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = newUser.Name,
            Email = newUser.Email,
            Balance = 0,
            Status = "Active",
            Role = "User"
        };
        user.Passwordhash = _passwordHasher.HashPassword(user, newUser.Password); // Need to pass the user into this

        if (EmailExists(newUser.Email))
        {
            throw new ErrorException("Email", "Email already exists");
        }

        var createdUser = _repository.CreateUserDB(user);
        return UserResponseDTO.FromEntity(createdUser, _jwtManager);
    }

    
    public UserResponseDTO Login(UserLoginRequestDTO userLoginRequest)
    {
       var userData = _repository.GetUserByEmail(userLoginRequest.Email);
       
       if (userData == null)
       {
           throw new ErrorException("User", "User does not exist");
       }

       if (_passwordHasher.VerifyHashedPassword(userData, userData.Passwordhash, userLoginRequest.Password) ==
           PasswordVerificationResult.Failed)
       {
           throw new ErrorException("Password", "Password does not match");
       }
       
       return UserResponseDTO.FromEntity(userData, _jwtManager);
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
        var jwtData = _jwtManager.IsJWTValid(jwtToken);
        var uuidClaim = jwtData.Claims.FirstOrDefault(claim => claim.Type == "uuid");
        var userData = _repository.GetUserById(uuidClaim.Value);
        
        if (userData == null)
        {
            throw new ErrorException("User", "User does not exist");
        }
        
        if (!roles.Contains(userData.Role))
        {
            throw new ErrorException("Authorization", "User does not have the required role.");
        }
    }
    
    
    private bool EmailExists(string email)
    {
        return _repository.EmailAlreadyExists(email);
    }
}