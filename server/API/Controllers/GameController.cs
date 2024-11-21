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
}