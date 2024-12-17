
using System.Globalization;
using API.Exceptions;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Types.Enums;
using Service.Services.Interfaces;
using Service.TransferModels.Responses;

namespace Service.Services;

public class GameService(IGameRepository gameRepository) : IGameService
{
    public GameResponseDTO NewGameFromMonday(int prize)
    {
        var game = new Game();

        // Get the current date
        var today = DateTime.Now;

        // Calculate the last Monday
        int daysSinceMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
        if (daysSinceMonday < 0)
        {
            daysSinceMonday += 7; // Adjust for Sundays
        }
        var lastMonday = today.AddDays(-daysSinceMonday);

        // Set the game's date to the last Monday
        game.Id = Guid.NewGuid();
        game.Date = DateOnly.FromDateTime(lastMonday);
        game.Prizepool = prize;
        game.Status = GameStatus.Active;
        
        var activeGame = gameRepository.GetActiveGame();
        if (activeGame != null)
        {
            activeGame.Status = GameStatus.Inactive;
        }
        
        var gameResponse = new GameResponseDTO().FromGame(gameRepository.NewGame(game,activeGame));
        return gameResponse;
    }
    
    public GameResponseDTO NewGame(int prize)
    {
        var game = new Game();
        
        game.Id = Guid.NewGuid();
        game.Date = DateOnly.FromDateTime(DateTime.Now);
        game.Prizepool = prize;
        game.Status = GameStatus.Active;
        
        var activeGame = gameRepository.GetActiveGame();
        if (activeGame != null)
        {
            activeGame.Status = GameStatus.Inactive;
        }
        
        var gameResponse = new GameResponseDTO().FromGame(gameRepository.NewGame(game,activeGame));
        return gameResponse;
    }

    public bool IsAnyGame()
    {
        var activeGame = gameRepository.GetActiveGame();
        if (activeGame != null)
        {
            return true;
        }
        return false;
    }
    
    public Guid? GetActiveGameId()
    {
        var activeGame = gameRepository.GetActiveGame();
        return activeGame?.Id; 
    }
    
    public WinningNumbersResponseDTO SetWinningNumbers(Guid gameId, int[] winningNumbers)
    {
        if (winningNumbers.Length != 3)
        {
            throw new Exception("Winning Numbers must be 3!");
        }
        
        var game = gameRepository.GetActiveGame();
        if (game == null || game.Id != gameId)
        {
            throw new ErrorException("Game", "Game not found or not active.");
        }

        var winningNumbersEntities = winningNumbers.Select(num => new WinningNumbers
        {
            Id = Guid.NewGuid(),
            GameId = gameId,
            Number = num
        }).ToList();

        gameRepository.AddWinningNumbers(winningNumbersEntities);

        var winningNumbersList = winningNumbersEntities.Select(e => e.Number).ToList();

        return WinningNumbersResponseDTO.FromGame(game, winningNumbersList);
    }
    
    public List<WinnerResponseDTO> GetWinners(Guid gameId)
    {
        var winners = gameRepository.GetWinnersWithGame(gameId);
        return winners.Select(w => new WinnerResponseDTO
        {
            UserName = w.User.Name, 
            WonAmount = w.Wonamount,
            WeekNumber = ISOWeek.GetWeekOfYear(w.Game.Date.ToDateTime(new TimeOnly(0, 0)))
        }).ToList();
    }
    
    public void UpdatePrizePool(decimal newPrizePool)
    {
        gameRepository.UpdatePrizePool(newPrizePool);
    }
}