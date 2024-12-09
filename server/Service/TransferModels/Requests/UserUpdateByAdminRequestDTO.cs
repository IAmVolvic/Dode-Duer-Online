using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataAccess.Types.Enums;

namespace Service.TransferModels.Requests;

public class UserUpdateByAdminRequestDTO
{
    [Required]
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
    
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; }
    
    [Phone(ErrorMessage = "Invalid phone number format.")]
    [MinLength(8, ErrorMessage = "Phone number is too short.")]
    [MaxLength(8, ErrorMessage = "Phone number is too long.")]
    public string? PhoneNumber { get; set; }
    
    [MinLength(5, ErrorMessage = "Password must be at least 5 characters.")]
    [MaxLength(32, ErrorMessage = "Password must be between 5 and 32 characters.")]
    public string? Password { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserEnrolled? EnrolledStatus { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserStatus? UserStatus { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole? UserRole { get; set; }
}