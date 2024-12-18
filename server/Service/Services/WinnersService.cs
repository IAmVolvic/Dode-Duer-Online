using DataAccess.Interfaces;
using Service.Services.Interfaces;
using Service.TransferModels.Responses;

namespace Service.Services;

public class WinnersService(IWinnersRepository winnersRepository, IBoardService boardService) : IWinnersService
{

    public List<WinnersDto> GetWinners(Guid gameId)
    {
        var winningBoards  = boardService.GetWinningBoardsFromGame(gameId);
        var winners = winnersRepository.GetWinners(gameId);
        return winners.Select(w => new WinnersDto().FromWinner(w,winningBoards)).ToList();
    }
}