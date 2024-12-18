using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using DataAccess.Models;
using DataAccess.Types.Enums;

namespace Service.TransferModels.Responses
{
    public class MyBoards
    {
        public Guid GameId { get; set; }
        public DateOnly StartDate { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GameStatus Status { get; set; }
        
        public DateTime? EndDate { get; set; }
        public List<UserBoard> Boards { get; set; }

        public static MyBoards FromEntity(Game game, List<UserBoard> boards)
        {
            return new MyBoards
            {
                GameId = game.Id,
                StartDate = game.Date,
                Status = game.Status,
                EndDate = game.Enddate,
                Boards = boards
            };
        }
    }

    public class UserBoard
    {
        public Guid BoardId { get; set; }
        public DateOnly DateOfPurchase { get; set; }

        [MinLength(5)]
        [MaxLength(8)]
        public List<int?> Numbers { get; set; }

        public int WinningAmount { get; set; }
        
        
        public static UserBoard FromEntity(Board userBoard, int winAmount)
        {
            return new UserBoard
            {
                BoardId = userBoard.Id,
                DateOfPurchase = userBoard.Dateofpurchase,
                Numbers = userBoard.Chosennumbers?.Select(n => n.Number).ToList() ?? new List<int?>(),
                WinningAmount = winAmount,
            };
        }
    }
}