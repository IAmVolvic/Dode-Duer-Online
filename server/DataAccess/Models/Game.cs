using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataAccess.Models;

public partial class Game
{
    public Guid Id { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Prizepool must be greater than or equal to 0.")]
    public decimal? Prizepool { get; set; }

    public DateOnly Date { get; set; }

    public string? Winningnumbers { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();

    public virtual ICollection<Winner> Winners { get; set; } = new List<Winner>();
}
