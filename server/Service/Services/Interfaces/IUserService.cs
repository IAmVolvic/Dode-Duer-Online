using DataAccess.Models;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IUserService
{
    public AuthorizedUserResponseDTO Signup(UserSignupRequestDTO newUser);
    public UserResponseDTO Login(UserLoginRequestDTO userLoginRequest);

    public void NewAdmin(User newUser);
}