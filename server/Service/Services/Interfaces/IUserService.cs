using DataAccess.Models;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IUserService
{
    public AuthorizedUserResponseDTO Signup(UserSignupRequestDTO newUser);
    public UserResponseDTO Login(UserLoginRequestDTO userLoginRequest);

    public AuthorizedUserResponseDTO EnrollUser(Guid userId, UserEnrollmentRequestDTO newPassword);

    public void NewAdmin(User newUser);
    
    public AuthorizedUserResponseDTO UpdateUser(Guid userId, UserUpdateRequestDTO userUpdateRequest);

    public AuthorizedUserResponseDTO UpdateUserByAdmin(UserUpdateByAdminRequestDTO userUpdateRequest);
    
    public AuthorizedUserResponseDTO[] GetUsers();
    public decimal UpdateUserBalance(decimal cost, Guid userId);

}