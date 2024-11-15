using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Service.TransferModels.Requests;

namespace Service.Services.Interfaces;

public interface IUserService
{
    public string CreateNewUser(UserSignupDTO newUser);
    public Boolean Login(string JWT);
}