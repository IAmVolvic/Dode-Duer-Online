using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IUserRepository
{
    public User CreateUserDB(User newUser);
}