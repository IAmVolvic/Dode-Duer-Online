using System.Text.Json.Serialization;
using DataAccess.Models;
using DataAccess.Types.Enums;

namespace Service.TransferModels.Responses;

public class AuthorizedUserResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public decimal Balance { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserEnrolled Enrolled { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserStatus Status { get; set; }
    
    
    public static AuthorizedUserResponseDTO FromEntity(User user)
    {
        return new AuthorizedUserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.Phonenumber,
            Balance = user.Balance,
            Role = user.Role,
            Enrolled = user.Enrolled,
            Status = user.Status
        };
    }
}