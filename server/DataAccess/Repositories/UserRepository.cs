using DataAccess.Contexts;
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

    
    public User GetUserByEmail(string email)
    {
        return context.Users.FirstOrDefault(u => u.Email == email);
    }

    
    public User GetUserById(string userId)
    {
        return context.Users.FirstOrDefault(u => u.Id == Guid.Parse(userId));
    }
    
    
    
    public Boolean EmailAlreadyExists(string email)
    {
        return context.Users.Any(u => u.Email == email);
    }
}