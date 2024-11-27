using DataAccess.Models;
using DataAccess.Types.Enums;
using Service.Services.Interfaces;

namespace API.Automation;

public class GameStart(IGameService gameService)
{
    public void StartGame()
    {
        if (gameService.IsAnyGame())
        {
            return;
        }
        gameService.NewGameFromMonday(0);
    }
}