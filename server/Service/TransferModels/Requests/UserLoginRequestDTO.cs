using System.ComponentModel.DataAnnotations;

namespace Service.TransferModels.Requests;

public class UserLoginRequestDTO
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "User password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    [MaxLength(32, ErrorMessage = "Password must be between 6 and 32 characters.")]
    public string Password { get; set; } = null!;
}