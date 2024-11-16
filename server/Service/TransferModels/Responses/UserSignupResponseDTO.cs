using System.ComponentModel.DataAnnotations;
using DataAccess.Models;
using Service.Security;

namespace Service.TransferModels.Responses;

public class UserSignupResponseDTO
{
    [Required(ErrorMessage = "UserId is required.")]
    public string Id { get; set; } = null!;
    
    [Required(ErrorMessage = "JWT token is required.")]
    public string JWT { get; set; } = null!;

    
    public static UserSignupResponseDTO FromEntity(User user, IJWTManager jwtManager)
    {
        return new UserSignupResponseDTO
        {
            Id = user.Id.ToString(),
            JWT = jwtManager.CreateJWT(user)
        };
    }
}