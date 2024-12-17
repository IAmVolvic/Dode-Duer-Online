using System.Text.Json.Serialization;
using DataAccess.Models;
using DataAccess.Types.Enums;

namespace Service.TransferModels.Responses;

public class GameResponseDTO
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    
    public Winner[] Winners {get; set;}
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GameStatus Status { get; set; }
    public DateTime? Enddate { get; set; }

    public GameResponseDTO FromGame(Game game)
    {
        return new GameResponseDTO()
        {
            Id = game.Id,
            Date = game.Date,
            Status = game.Status,
            Enddate = game.Enddate,
        };
    }
}