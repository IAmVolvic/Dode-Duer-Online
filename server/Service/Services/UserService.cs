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

        PhoneNumberExists(newUser.PhoneNumber);
        EmailExists(newUser.Email);
        
        // SMTP email to user letting them know they had been signed up
        Console.WriteLine(randomPassword);
        
        _repository.CreateUserDb(user);
        return AuthorizedUserResponseDTO.FromEntity(user);
    }
    
    public UserResponseDTO Login(UserLoginRequestDTO userLoginRequest)
    {
       var userData = UserByEmail(userLoginRequest.Email);

       if (_passwordHasher.VerifyHashedPassword(userData, userData.Passwordhash, userLoginRequest.Password) ==
           PasswordVerificationResult.Failed)
       {
           throw new ErrorException("Password", "Password does not match");
       }
       
       return UserResponseDTO.FromEntity(userData, _jwtManager);
    }

    public AuthorizedUserResponseDTO EnrollUser(Guid userId, UserEnrollmentRequestDTO data)
    {
        var userData = UserById(userId.ToString());
        
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
        
        EmailExists(newUser.Email);
        PhoneNumberExists(newUser.Phonenumber);
        
        _repository.CreateUserDb(newUser);
    }
    
    
    private User UserById(string userId)
    {
        var user = _repository.GetUserById(userId);
        
        if (user == null)
        {
            throw new ErrorException("User", "User does not exist");
        }
        
        return user;
    }
    
    private User UserByEmail(string email)
    {
        var user = _repository.GetUserByEmail(email);
        
        if (user == null)
        {
            throw new ErrorException("User", "User does not exist");
        }
        
        return user;
    }
    
    private void EmailExists(string email)
    {
        if (_repository.EmailAlreadyExists(email))
        {
            throw new ErrorException("Email", "Email already exists");
        }
    }
    
    private void PhoneNumberExists(string phoneNumber)
    {
        if (_repository.PhoneNumberAlreadyExists(phoneNumber))
        {
            throw new ErrorException("Phone", "Phone number already exists");
        }
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