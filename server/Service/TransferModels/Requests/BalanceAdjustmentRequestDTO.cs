using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataAccess.Types.Enums;
using Microsoft.EntityFrameworkCore.Storage.Json;
using TransactionStatus = System.Transactions.TransactionStatus;

namespace Service.TransferModels.Requests;

public class BalanceAdjustmentRequestDTO
{
    [Required(ErrorMessage = "Transaction Id is required.")]
    public string TransactionId { get; set; } = null!;
    
    [Required(ErrorMessage = "Amount is required.")]
    [Range(0, 10000, ErrorMessage = "Amount must be between 0 and 10,000.")]
    public decimal Amount { get; set; }
    
    [Required(ErrorMessage = "Adjustment type is required.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionAdjustment Adjustment { get; set; }
    
    [Required(ErrorMessage = "Adjustment type is required.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionStatus TransactionStatus { get; set; }
}