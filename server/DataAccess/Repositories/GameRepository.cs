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
        var gameId = winningNumbers.First().GameId;

        var existingWinningNumbers = context.WinningNumbers
            .Where(wn => wn.GameId == gameId)
            .ToList();

        context.WinningNumbers.RemoveRange(existingWinningNumbers);

        foreach (var winningNumber in winningNumbers)
        {
            winningNumber.GameId = gameId;
            context.WinningNumbers.Add(winningNumber);
        }
        
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

    public List<Game> GetAllGames()
    {
        return context.Games.ToList();
    }

    public Game GetGameById(Guid gameId)
    {
        return context.Games.Find(gameId);
    }

    public List<WinningNumbers> GetWinningNumbers(Guid gameId)
    {
        return context.WinningNumbers.Where(wn => wn.GameId == gameId).ToList();
    }
}

