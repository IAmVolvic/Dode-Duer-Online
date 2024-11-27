using DataAccess.Models;
using DataAccess.Types.Enums;

namespace Service.TransferModels.Responses;

public class TransactionResponseDTO
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    
    public string PhoneNumber { get; set; } = null!;
    
    public string Username { get; set; } = null!;
    
    public string TransactionNumber { get; set; } = null!;
    
    public string TransactionStatus { get; set; }
    
    
    public static TransactionResponseDTO FromEntity(Transaction transaction, User user)
    {
        return new TransactionResponseDTO
        {
            Id = transaction.Id,
            UserId = transaction.Userid,
            Username = user.Name,
            PhoneNumber = user.Phonenumber,
            TransactionNumber = transaction.Transactionnumber,
            TransactionStatus = transaction.Transactionstatus.ToString()
        };
    }
}