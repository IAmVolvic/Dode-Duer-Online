using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.Json;

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
    public AdjustmentType Adjustment { get; set; }
    
    [Required(ErrorMessage = "Adjustment type is required.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BalanceAdjustType TransactionStatus { get; set; }
    
    
    
    public enum AdjustmentType
    {
        Deposit,
        Deduct
    }
    
    public enum BalanceAdjustType
    {
        Pending,
        Approved,
        Rejected
    }
}