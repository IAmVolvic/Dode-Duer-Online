using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IUserRepository
{
    public User CreateUserDb(User newUser);

    public User GetUserByEmail(string email);

    public User GetUserById(string userId);
    
    public Boolean EmailAlreadyExists(string email);

    public Boolean PhoneNumberAlreadyExists(string phoneNumber);

    public Boolean AdminAlreadyExists();

    public User UpdateUserDb(User user);

    public User[] GetUsers();

    public decimal GetUserBalance(User user);
}