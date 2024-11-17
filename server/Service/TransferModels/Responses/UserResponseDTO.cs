using System.ComponentModel.DataAnnotations;
using DataAccess.Models;
using Service.Security;

namespace Service.TransferModels.Responses;

public class UserResponseDTO
{
    [Required(ErrorMessage = "UserId is required.")]
    public string Id { get; set; } = null!;
    
    [Required(ErrorMessage = "JWT token is required.")]
    public string JWT { get; set; } = null!;

    
    public static UserResponseDTO FromEntity(User user, IJWTManager jwtManager)
    {
        return new UserResponseDTO
        {
            Id = user.Id.ToString(),
            JWT = jwtManager.CreateJWT(user)
        };
    }
}