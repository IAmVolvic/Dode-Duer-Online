using System.ComponentModel.DataAnnotations;

namespace Service.TransferModels.Requests;

public class UserSignupCompleteRequestDTO
{
        
    [Required(ErrorMessage = "User password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    [MaxLength(32, ErrorMessage = "Password must be between 6 and 32 characters.")]
    public string Password { get; set; } = null!;
}