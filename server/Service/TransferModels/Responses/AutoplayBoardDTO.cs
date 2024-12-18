using System.ComponentModel.DataAnnotations;
using DataAccess.Models;

namespace Service.TransferModels.Responses;

public class AutoplayBoardDTO
{
    public Guid Id { get; set; }

    public Guid Userid { get; set; }

    [MinLength(5)]
    [MaxLength(8)]
    public List<int> Numbers { get; set; } 
    
    public int LeftToPlay { get; set; }

    public AutoplayBoardDTO FromBoard(BoardAutoplay board)
    {
        return new AutoplayBoardDTO()
        {
            Id = board.Id,
            Userid = board.UserId,
            Numbers = board.ChosenNumbersAutoplays.Select(n => n.Number).ToList(),
            LeftToPlay = board.LeftToPlay
        };
    }
}