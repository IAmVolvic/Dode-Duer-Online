using System.ComponentModel.DataAnnotations;

namespace Service.TransferModels.Requests;

public class PlayAutoplayBoardDTO
{
    [Required]
    public Guid Userid { get; set; }
    [MinLength(5)]
    [MaxLength(8)]
    [RangeForList(1, 16)]
    public List<int> Numbers { get; set; }
    public int LeftToPlay {get; set;}
}