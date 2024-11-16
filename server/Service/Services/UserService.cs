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

    public UserSignupResponseDTO CreateNewUser(UserSignupRequestDTO newUser)
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
            throw new ErrorExcep("Email", "Email already exists");
        }

        var createdUser = _repository.CreateUserDB(user);
        return UserSignupResponseDTO.FromEntity(createdUser, _jwtManager);
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