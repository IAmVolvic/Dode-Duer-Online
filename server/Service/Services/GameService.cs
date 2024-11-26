using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Types.Enums;
using Service.Services.Interfaces;
using Service.TransferModels.Responses;

namespace Service.Services;

public class GameService(IGameRepository gameRepository) : IGameService
{
    public GameResponseDTO NewGame(int prize)
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
}