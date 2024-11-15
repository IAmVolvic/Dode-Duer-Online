using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Service.Security;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;

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

    public string CreateNewUser(UserSignupDTO newUser)
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
            throw new Exception("Email already exists");
        }

        _repository.CreateUserDB(user);
        return _jwtManager.CreateJWT(user);
    }


    public Boolean Login(string JWT)
    {
        Console.Write(_jwtManager.IsJWTValid(JWT));
        return true;
    }
    
    private bool EmailExists(string email)
    {
        return _repository.EmailAlreadyExists(email);
    }
}