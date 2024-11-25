using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IUserRepository
{
    public User CreateUserDB(User newUser);

    public User GetUserByEmail(string email);

    public User GetUserById(string userId);
    
    public Boolean EmailAlreadyExists(string email);

    public User UpdateUserDB(User user);
}