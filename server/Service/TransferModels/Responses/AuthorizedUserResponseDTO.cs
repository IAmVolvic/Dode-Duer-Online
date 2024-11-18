using DataAccess.Models;

namespace Service.TransferModels.Responses;

public class AuthorizedUserResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal Balance { get; set; }
    public string Role { get; set; }
    public string Status { get; set; }
    
    
    public static AuthorizedUserResponseDTO FromEntity(User user)
    {
        return new AuthorizedUserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Balance = user.Balance,
            Role = user.Role,
            Status = user.Status
        };
    }
}