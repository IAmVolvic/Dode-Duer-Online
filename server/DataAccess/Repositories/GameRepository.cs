using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Types.Enums;


namespace DataAccess.Repositories;

public class GameRepository(LotteryContext context) : IGameRepository
{
    public Game NewGame(Game game, Game activeGame)
    {
        if (activeGame == null)
        {
            context.Games.Add(game);
            context.SaveChanges();
        }
        else
        {
            context.Games.Add(game);
            context.Games.Update(activeGame);
            context.SaveChanges();
        }
        return game;
    }

    public Game GetActiveGame()
    {
        return context.Games.FirstOrDefault(g => g.Status == GameStatus.Active);
    }
    
    public void AddWinningNumbers(List<WinningNumbers> winningNumbers)
    {
        context.WinningNumbers.AddRange(winningNumbers);
        context.SaveChanges();
    }

    public void UpdatePrizePool(decimal newPrizePool)
    {
        var game = context.Games.FirstOrDefault(g => g.Status == GameStatus.Active);
        if (game != null)
        {
            game.Prizepool = newPrizePool;
            context.SaveChanges();
        }
        else
        {
            throw new InvalidOperationException("Active game is not available.");
        }
    }
}

