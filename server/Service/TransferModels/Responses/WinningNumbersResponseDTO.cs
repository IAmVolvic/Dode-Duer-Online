using DataAccess.Models;
using DataAccess.Types.Enums;

namespace Service.TransferModels.Responses;

public class WinningNumbersResponseDTO
{
    public Guid Gameid { get; set; }
    public int WinningNumbers { get; set; }
    public GameStatus Status { get; set; }

    public static WinningNumbersResponseDTO FromGame(Game game, int winningNumber)
    {
        return new WinningNumbersResponseDTO()
        {
            Gameid = game.Id,
            WinningNumbers = winningNumber,
            Status = game.Status
        };
    }

}