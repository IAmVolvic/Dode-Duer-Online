using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IUserService
{
    public UserResponseDTO Signup(UserSignupRequestDTO newUser);
    public UserResponseDTO Login(UserLoginRequestDTO userLoginRequest);
}