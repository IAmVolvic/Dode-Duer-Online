using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IGameRepository
{
    public Game NewGame(Game game, Game activeGame);
    public Game GetActiveGame();
    void AddWinningNumbers(List<WinningNumbers> winningNumbers);
    public void UpdatePrizePool(decimal newPrizePool);
    public List<Game> GetAllGames();
    public Game GetGameById(Guid gameId);
    public List<WinningNumbers> GetWinningNumbers(Guid gameId);
}