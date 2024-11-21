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
    [Route("/Play")]
    public ActionResult<BoardResponseDTO> PlayBoard([FromBody] PlayBoardDTO playBoardDTO)
    {
        var response = boardService.PlayBoard(playBoardDTO);
        return  Ok(response);
    }
}