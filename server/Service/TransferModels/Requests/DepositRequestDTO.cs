using System.ComponentModel.DataAnnotations;

namespace Service.TransferModels.Requests;

public class DepositRequestDTO
{
    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    [MinLength(8, ErrorMessage = "Phone number is too short.")]
    [MaxLength(8, ErrorMessage = "Phone number is too long.")]
    public string PhoneNumber { get; set; }
    
    [Required(ErrorMessage = "Transaction number is required.")]
    [MinLength(8, ErrorMessage = "Transaction number is too short.")]
    [MaxLength(15, ErrorMessage = "Transaction number is too long.")]
    public string TransactionNumber { get; set; }
    
    [Required(ErrorMessage = "MobilePay username is required.")]
    public string MobilePayUsername { get; set; } = null!;
}