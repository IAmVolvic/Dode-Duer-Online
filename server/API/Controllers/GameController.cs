using API.Attributes;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace API.Controllers;


[ApiController]
[Route("[controller]")]
public class GameController(IGameService gameService, IBoardService boardService) : ControllerBase
{
    [HttpPost]
    [Route("NewGame")]
    [Rolepolicy("Admin")]
    public ActionResult<GameResponseDTO> NewGame([FromBody] int prize)
    {
        var game = gameService.NewGame(prize);
        boardService.PlayAllAutoplayBoards();
        return Ok(game);
    }

    [HttpPost]
    [Route("NewGameFromMonday")]
    [Rolepolicy("Admin")]
    public ActionResult<GameResponseDTO> NewGameFromMonday([FromBody] int prize)
    {
        return Ok(gameService.NewGameFromMonday(prize));
    }

    [HttpGet]
    [Route("getAllGames")]
    public ActionResult<List<GameResponseDTO>> GetAllGames()
    {
        var result = gameService.GetAllGames();
        return Ok(result);
    }

    [HttpPost]
    [Route("winningNumbers")]
    [Rolepolicy("Admin")]
    public ActionResult<WinningNumbersResponseDTO> SetWinningNumbers([FromBody] WinningNumbersRequestDTO data)
    {
        return Ok(gameService.SetWinningNumbers(data));
    }
    

}