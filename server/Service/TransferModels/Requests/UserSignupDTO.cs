using System.ComponentModel.DataAnnotations;

namespace Service.TransferModels.Requests;

public class UserSignupDTO
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "User password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; } = null!;
}