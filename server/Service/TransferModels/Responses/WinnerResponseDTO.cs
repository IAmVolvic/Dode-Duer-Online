using DataAccess.Models;
using DataAccess.Types.Enums;

namespace Service.TransferModels.Responses;

public class WinnerResponseDTO
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public decimal Wonamount { get; set; }
    public int WeekNumber { get; set; }
}