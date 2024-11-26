using System.ComponentModel.DataAnnotations;

namespace Service.TransferModels.Requests;

public class UserSignupRequestDTO
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    [MinLength(8, ErrorMessage = "Phone number is too short.")]
    [MaxLength(8, ErrorMessage = "Phone number is too long.")]
    public string PhoneNumber { get; set; } = null!;
}