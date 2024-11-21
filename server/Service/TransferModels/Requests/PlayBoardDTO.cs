using System.ComponentModel.DataAnnotations;

namespace Service.TransferModels.Requests;

public class PlayBoardDTO
{
    public Guid Userid { get; set; }
    public DateOnly Dateofpurchase { get; set; }
    [MinLength(5)]
    [MaxLength(8)]
    public List<int> Numbers { get; set; }
}