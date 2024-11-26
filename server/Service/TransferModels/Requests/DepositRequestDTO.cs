using System.ComponentModel.DataAnnotations;

namespace Service.TransferModels.Requests;

public class DepositRequestDTO
{
    [Required(ErrorMessage = "Transaction number is required.")]
    [MinLength(8, ErrorMessage = "Transaction number is too short.")]
    [MaxLength(15, ErrorMessage = "Transaction number is too long.")]
    public string TransactionNumber { get; set; } = null!;
}