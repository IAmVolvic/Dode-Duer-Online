using API.Attributes;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.TransferModels.Responses;

namespace API.Controllers;


[ApiController]
[Route("[controller]")]
public class GameController(IGameService gameService) : ControllerBase
{
    [HttpPost]
    [Route("NewGame")]
    [Rolepolicy("Admin")]
    public ActionResult<GameResponseDTO> NewGame([FromBody] int prize)
    {
        return Ok(gameService.NewGame(prize));
    }
    
    [HttpPost]
    [Route("{gameId}/winning-numbers")]
    [Rolepolicy("Admin")]
    public ActionResult<WinningNumbersResponseDTO> AddWinningNumbers(Guid gameId, [FromBody] int winningNumber)
    {
        var result = gameService.SetWinningNumbers(gameId, winningNumber);
        return Ok(result);
    }
}