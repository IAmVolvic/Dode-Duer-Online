using System.ComponentModel.DataAnnotations;

namespace Service.TransferModels.Requests;

public class WinningNumbersRequestDTO
{
    [Required]
    public Guid GameId { get; set; }
    
    [Required]
    [MinLength(3, ErrorMessage = "Winning Numbers must be at least 3 numbers.")]
    [MaxLength(3, ErrorMessage = "Winning Numbers must be no more than 3 numbers.")]
    public List<int> WinningNumbers { get; set; }
}