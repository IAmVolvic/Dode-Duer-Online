using System.Globalization;
using DataAccess.Models;
using DataAccess.Types.Enums;

namespace Service.TransferModels.Responses
{
    public class WinnerResponseDTO
    {
        public string UserName { get; set; }
        public decimal WonAmount { get; set; }
        public int WeekNumber { get; set; }

        public static WinnerResponseDTO FromBoard(Board board, decimal wonAmount)
        {
            return new WinnerResponseDTO
            {
                UserName = board.User.Name,
                WonAmount = wonAmount,
                WeekNumber = ISOWeek.GetWeekOfYear(board.Game.Date.ToDateTime(new TimeOnly(0, 0)))
            };
        }
    }
}