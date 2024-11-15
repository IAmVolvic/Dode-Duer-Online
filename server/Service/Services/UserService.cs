using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;

namespace Service.Services;

public class UserService : IUserService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _repository;
    
    public UserService(IPasswordHasher<User> passwordHasher, IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _repository = userRepository;
    }

    public User CreateNewUser(UserSignupDTO newUser)
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
        
        return _repository.CreateUserDB(user);
    }
}