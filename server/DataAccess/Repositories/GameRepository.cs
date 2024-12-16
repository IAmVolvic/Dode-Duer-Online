using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Types.Enums;
using Microsoft.EntityFrameworkCore;


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
    
    public List<Winner> GetWinnersWithGame(Guid gameId)
    {
        return context.Winners
            .Where(w => w.Gameid == gameId)
            .Include(w => w.Game)
            .Include(w => w.User)
            .Select(w => new Winner
            {
                Id = w.Id,
                Gameid = w.Gameid,
                User = w.User,
                Game = w.Game,
                //Wonamount = CalculateWonamount(w)
            })
            .ToList();
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

