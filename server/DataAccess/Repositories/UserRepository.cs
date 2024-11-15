using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Repositories;

public class UserRepository(LotteryContext context) : IUserRepository
{
    public User CreateUserDB(User newUser)
    {
        context.Users.Add(newUser);
        context.SaveChanges();
        return newUser;
    }
}