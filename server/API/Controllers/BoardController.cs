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
        return Ok(response);
    }

    [HttpPost]
    [Route("Autoplay")]
    [Authenticated]
    public ActionResult<AutoplayBoardDTO> AutoplayBoard([FromBody] PlayAutoplayBoardDTO playAutoplayBoardDTO)
    {
        var response = boardService.AutoplayBoard(playAutoplayBoardDTO);
        return Ok(response);
    }

[HttpGet]
    [Route("GetBoards")]
    [Rolepolicy("Admin")]
    public ActionResult<List<BoardResponseDTO>> GetAllBoards()
    {
        var response = boardService.GetBoards();
        return Ok(response);
    }

    [HttpGet]
    [Route("GetBoardsFromGame/{gameId}")]
    [Rolepolicy("Admin")]
    public ActionResult<List<BoardGameResponseDTO>> GetBoardsFromGame([FromRoute] Guid gameId)
    {
        var response = boardService.GetBoardsFromGame(gameId);
        return Ok(response);
    }
}