﻿using API.Attributes;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
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
    
    [HttpPost]
    [Route("winning-numbers")]
    [Rolepolicy("Admin")]
    public ActionResult<WinningNumbersResponseDTO> AddWinningNumbers(Guid gameId, [FromBody] int[] winningNumbers)
    {
        var result = gameService.SetWinningNumbers(gameId, winningNumbers);
        return Ok(result);
    }

    [HttpGet]
    [Route("getAllGames")]
    public ActionResult<List<GameResponseDTO>> GetAllGames()
    {
        var result = gameService.GetAllGames();
        return Ok(result);
    }
}