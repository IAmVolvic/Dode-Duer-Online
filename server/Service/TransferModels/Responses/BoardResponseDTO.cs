using System.ComponentModel.DataAnnotations;
using DataAccess.Models;

namespace Service.TransferModels.Responses;

public class BoardResponseDTO
{
    public Guid Id { get; set; }

    public Guid Userid { get; set; }

    public Guid Gameid { get; set; }

    public decimal Price { get; set; }

    public DateOnly Dateofpurchase { get; set; }

    [MinLength(5)]
    [MaxLength(8)]
    public List<int?> Numbers { get; set; }

    public BoardResponseDTO FromBoard(Board board)
    {
        return new BoardResponseDTO()
        {
            Id = board.Id,
            Userid = board.Userid,
            Gameid = board.Gameid,
            Price = board.Price.Price1,
            Numbers = board.Chosennumbers.Select(n => n.Number).ToList(),
            Dateofpurchase = board.Dateofpurchase
        };
    }
}