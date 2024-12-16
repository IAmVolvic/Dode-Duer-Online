using DataAccess.Models;
using DataAccess.Types.Enums;

namespace Service.TransferModels.Responses;

public class WinningNumbersResponseDTO
{
    public Guid Gameid { get; set; }
    public List<int> Winningnumbers { get; set; }
    public GameStatus Status { get; set; }

    public static WinningNumbersResponseDTO FromGame(Game game, List<int> winningNumbers)
    {
        return new WinningNumbersResponseDTO
        {
            Gameid = game.Id,
            Winningnumbers = winningNumbers,
            Status = game.Status
        };
    }
}