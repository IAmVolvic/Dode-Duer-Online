using DataAccess.Models;

namespace Service.TransferModels.Responses;

public class WinnersDto
{
    public Guid Gameid { get; set; }
    public string Name { get; set; }
    public Guid UserId { get; set; }
    public decimal Prize { get; set; } = 0;
    public int NumberOfWinningBoards { get; set; }
    public List<BoardGameResponseDTO> WinningBoards { get; set; } = new List<BoardGameResponseDTO>();

    public Winner ToWinner()
    {
        return new Winner()
        {
            Gameid = Gameid,
            Id = Guid.NewGuid(),
            Userid = UserId,
            Wonamount = Prize
        };
    }

    public WinnersDto FromWinner(Winner winner, List<BoardGameResponseDTO> boards)
    {
        var winersBoards = new List<BoardGameResponseDTO>();
        foreach (var board in boards)
        {
            if (board.userId == winner.Userid)
            {
                winersBoards.Add(board);
            }
        }

        return new WinnersDto()
        {
            Gameid = winner.Gameid,
            Prize = winner.Wonamount,
            Name = winner.User.Name,
            UserId = winner.Userid,
            WinningBoards = winersBoards,
            NumberOfWinningBoards = winersBoards.Count
        };
    }
}