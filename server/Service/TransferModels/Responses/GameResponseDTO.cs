﻿using DataAccess.Models;
using DataAccess.Types.Enums;

namespace Service.TransferModels.Responses;

public class GameResponseDTO
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public GameStatus Status { get; set; }

    public GameResponseDTO FromGame(Game game)
    {
        return new GameResponseDTO()
        {
            Id = game.Id,
            Date = game.Date,
            Status = game.Status
        };
    }
}