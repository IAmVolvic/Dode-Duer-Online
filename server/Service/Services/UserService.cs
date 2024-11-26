using API.Exceptions;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Types.Enums;
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
            Status = UserStatus.Active,
            Role = UserRole.User,
        };
        user.Passwordhash = "abc"; /*_passwordHasher.HashPassword(user, newUser.Password);*/ // Need to pass the user into this

        if (EmailExists(newUser.Email))
        {
            throw new ErrorException("Email", "Email already exists");
        }

        var createdUser = _repository.CreateUserDb(user);
        return UserResponseDTO.FromEntity(createdUser, _jwtManager);
    }
    
    
    public void NewAdmin(User newUser)
    {
        Guid adminId = Guid.NewGuid();
        newUser.Id = adminId;
        newUser.Passwordhash = _passwordHasher.HashPassword(newUser, newUser.Passwordhash);

        if (_repository.AdminAlreadyExists())
        {
            return;
        }
        
        if (EmailExists(newUser.Email))
        {
            throw new ErrorException("Email", "Email already exists");
        }

        if (PhoneNumberExists(newUser.Phonenumber))
        {
            throw new ErrorException("Phone", "Phone number already exists");
        }

        _repository.CreateUserDb(newUser);
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
    
    
    private bool EmailExists(string email)
    {
        return _repository.EmailAlreadyExists(email);
    }
    
    private bool PhoneNumberExists(string phoneNumber)
    {
        return _repository.PhoneNumberAlreadyExists(phoneNumber);
    }
}