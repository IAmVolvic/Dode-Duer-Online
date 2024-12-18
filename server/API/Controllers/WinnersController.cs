using API.Attributes;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.TransferModels.Responses;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WinnersController(IBoardService boardService, IWinnersService winnersService) : ControllerBase
{
    [HttpGet]
    [Route("establishWinners/{gameId}")]
    [Rolepolicy("Admin")]
    public ActionResult<List<WinnersDto>> GetWinners([FromRoute] Guid gameId)
    {
        var result = boardService.EstablishWinners(gameId);
        return Ok(result);
    }
}