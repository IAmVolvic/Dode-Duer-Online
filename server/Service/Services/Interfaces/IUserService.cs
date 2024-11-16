using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IUserService
{
    public UserSignupResponseDTO CreateNewUser(UserSignupRequestDTO newUser);
    public Boolean Login(string JWT);
}