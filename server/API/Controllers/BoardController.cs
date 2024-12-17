using API.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class BoardController(IBoardService boardService) : ControllerBase
{
    [HttpPost]
    [Route("Play")]
    [Authenticated]
    public ActionResult<BoardResponseDTO> PlayBoard([FromBody] PlayBoardDTO playBoardDTO)
    {
        var response = boardService.PlayBoard(playBoardDTO);
        return  Ok(response);
    }

    [HttpGet]
    [Route("GetBoards")]
    [Rolepolicy("Admin")]
    public ActionResult<List<BoardResponseDTO>> GetAllBoards()
    {
        var response = boardService.GetBoards();
        return Ok(response);
    }
    
    [HttpPost("{gameId}/identify-winners")]
    public ActionResult<List<WinnerResponseDTO>> IdentifyWinners(Guid gameId, List<int> winningNumbers)
    {
        var winners = boardService.IdentifyWinners(gameId, winningNumbers);
        return Ok(winners);
    }
         

    [HttpGet("{gameId}/winners")]
   public ActionResult<List<WinnerResponseDTO>> GetWinners(Guid gameId)
    {
        var winners = boardService.GetWinners(gameId);
        return Ok(winners);
    }
}