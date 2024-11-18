using DataAccess.Models;

namespace Service.TransferModels.Responses;

public class GameResponseDTO
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string Status { get; set; }

    public GameResponseDTO FromGame(Game game)
    {
        return new GameResponseDTO()
        {
            Id = game.Id,
            Date = game.Date,
            Status = game.Status
        };
    }
}