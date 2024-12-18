using System.Text.Json.Serialization;
using DataAccess.Models;
using DataAccess.Types.Enums;

namespace Service.TransferModels.Responses;

public class GameResponseDTO
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public decimal Prize { get; set; }
    public decimal StartingPrizepool { get; set; }
    public Winner[] Winners {get; set;}
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GameStatus Status { get; set; }
    
    public DateTime? Enddate { get; set; }
    
    public List<int>? WinningNumbers { get; set; }

    public GameResponseDTO FromGame(Game game, List<int>? numbers)
    {
        return new GameResponseDTO()
        {
            Id = game.Id,
            Date = game.Date,
            Status = game.Status,
            Prize = game.Prizepool,
            Enddate = game.Enddate,
            WinningNumbers = numbers ?? new List<int>(),
            StartingPrizepool = game.StartingPrizepool,
        };
    }
}