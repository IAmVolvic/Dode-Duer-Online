using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IGameRepository
{
    public Game NewGame(Game game, Game activeGame);
    public Game GetActiveGame();
    public void AddWinningNumbers(List<WinningNumbers> winningNumbers);
    public List<Winner> GetWinnersWithGame(Guid gameId);
    void SaveWinners(List<Winner> winners);
    public void UpdatePrizePool(decimal newPrizePool);
    
}