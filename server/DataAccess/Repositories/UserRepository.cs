using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Repositories;

public class UserRepository(LotteryContext context) : IUserRepository
{
    public User CreateUserDb(User newUser)
    {
        context.Users.Add(newUser);
        context.SaveChanges();
        return newUser;
    }


    public User UpdateUserDb(User user)
    {
        var updatedUser = context.Users.Find(user.Id);
        
        if (user.Name != null)
        {
            updatedUser.Name = user.Name;
        }
        
        if (user.Email != null)
        {
            updatedUser.Email = user.Email;
        }
        
        if (user.Passwordhash != null)
        {
            updatedUser.Passwordhash = user.Passwordhash;
        }
        
        if (user.Balance != null)
        {
            updatedUser.Balance = user.Balance;
        }
        
        if (user.Role != null)
        {
            updatedUser.Role = user.Role;
        }
        
        if (user.Status != null)
        {
            updatedUser.Status = user.Status;
        }

        if (user.Enrolled != null)
        {
            updatedUser.Enrolled = user.Enrolled;
        }

        context.SaveChanges();
        return updatedUser;
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
    
    public Boolean PhoneNumberAlreadyExists(string phoneNumber)
    {
        return context.Users.Any(u => u.Phonenumber == phoneNumber);
    }
}