using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Service.TransferModels.Requests;

namespace Service.Services.Interfaces;

public interface IUserService
{
    public User CreateNewUser(UserSignupDTO newUser);
}