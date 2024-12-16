using System.ComponentModel.DataAnnotations;
using DataAccess.Models;

namespace Service.TransferModels.Responses;

public class BoardGameResponseDTO
{
    public Guid Id { get; set; }

    public virtual string User { get; set; }

    public DateOnly Dateofpurchase { get; set; }

    [MinLength(5)]
    [MaxLength(8)]
    public List<int?> Numbers { get; set; }

    public BoardGameResponseDTO FromBoard(Board board)
    {
        return new BoardGameResponseDTO()
        {
            Id = board.Id,
            User = board.User.Name,
            Numbers = board.Chosennumbers.Select(n => n.Number).ToList(),
            Dateofpurchase = board.Dateofpurchase
        };
    }
}