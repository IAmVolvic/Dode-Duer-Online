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
    public ActionResult<List<WinnersDto>> EstablishWinners([FromRoute] Guid gameId)
    {
        var result = boardService.EstablishWinners(gameId);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("getWinners/{gameId}")]
    [Rolepolicy("Admin")]
    public ActionResult<List<WinnersDto>> GetWinners([FromRoute] Guid gameId)
    {
        var result = winnersService.GetWinners(gameId);
        return Ok(result);
    }
}