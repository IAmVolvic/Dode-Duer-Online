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

    
    public AuthorizedUserResponseDTO Signup(UserSignupRequestDTO newUser)
    {
        Guid userId = Guid.NewGuid();
        var randomPassword = GenerateRandomString();
            
        var user = new User
        {
            Id = userId,
            Name = newUser.Name,
            Email = newUser.Email,
            Phonenumber = newUser.PhoneNumber,
            Balance = 0,
            Status = UserStatus.Active,
            Enrolled = UserEnrolled.False,
            Role = UserRole.User,
        };
        user.Passwordhash = _passwordHasher.HashPassword(user, randomPassword);

        if (PhoneNumberExists(newUser.PhoneNumber))
        {
            throw new ErrorException("Phone", "Phone number already exists");
        }
        
        if (EmailExists(newUser.Email))
        {
            throw new ErrorException("Email", "Email already exists");
        }
        
        // SMTP email to user letting them know they had been signed up
        Console.WriteLine(randomPassword);
        
        _repository.CreateUserDb(user);
        return AuthorizedUserResponseDTO.FromEntity(user);
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

    public AuthorizedUserResponseDTO EnrollUser(Guid userId, UserEnrollmentRequestDTO data)
    {
        var userData = _repository.GetUserById(userId.ToString());
        
        if (userData == null)
        {
            throw new ErrorException("User", "User does not exist");
        }
        
        if (userData.Enrolled == UserEnrolled.True)
        {
            throw new ErrorException("Enrollment", "User has already been enrolled");
        }
        
        userData.Passwordhash = _passwordHasher.HashPassword(userData, data.Password);
        userData.Enrolled = UserEnrolled.True;
        _repository.UpdateUserDb(userData);
        
        return AuthorizedUserResponseDTO.FromEntity(userData);
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
    
    private bool EmailExists(string email)
    {
        return _repository.EmailAlreadyExists(email);
    }
    
    private bool PhoneNumberExists(string phoneNumber)
    {
        return _repository.PhoneNumberAlreadyExists(phoneNumber);
    }
    
    private static string GenerateRandomString()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        int length = random.Next(5, 11);
        
        char[] stringChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }
        
        return new string(stringChars);
    }
}