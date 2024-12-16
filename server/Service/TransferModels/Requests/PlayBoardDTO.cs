using System.ComponentModel.DataAnnotations;
using DataAccess.Models;

namespace Service.TransferModels.Requests;

public class PlayBoardDTO
{
    [Required]
    public Guid Userid { get; set; }
    public DateOnly Dateofpurchase { get; set; }
    [MinLength(5)]
    [MaxLength(8)]
    [RangeForList(1, 16)]
    public List<int> Numbers { get; set; }

    public PlayBoardDTO fromAutoplay(BoardAutoplay autoplay)
    {
        return new PlayBoardDTO()
        {
            Userid = autoplay.UserId,
            Dateofpurchase = DateOnly.FromDateTime(DateTime.Today),
            Numbers = autoplay.ChosenNumbersAutoplays.Select(c => c.Number).ToList()
        };
    }
}