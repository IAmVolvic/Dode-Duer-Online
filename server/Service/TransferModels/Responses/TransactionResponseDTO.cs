using DataAccess.Models;

namespace Service.TransferModels.Responses;

public class TransactionResponseDTO
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    
    public string TransactionPhoneNumber { get; set; } = null!; // MobilePay phone number
    
    public string TransactionUsername { get; set; } = null!; // MobilePay Username
    
    public string TransactionNumber { get; set; } = null!; // MobilePay transaction number
    
    public string TransactionStatus { get; set; } = "Pending"; // Default status "Pending"
    
    
    public static TransactionResponseDTO FromEntity(Transaction transaction)
    {
        return new TransactionResponseDTO
        {
            Id = transaction.Id,
            UserId = transaction.Userid,
            /*TransactionPhoneNumber = transaction.TransactionPhoneNumber,
            TransactionUsername = transaction.TransactionUsername,
            TransactionNumber = transaction.TransactionNumber,
            TransactionStatus = transaction.TransactionStatus*/
        };
    }
}