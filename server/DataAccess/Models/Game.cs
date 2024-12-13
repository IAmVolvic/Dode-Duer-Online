using System;
using System.Collections.Generic;
using DataAccess.Types.Enums;

namespace DataAccess.Models;

public partial class Game
{
    public Guid Id { get; set; }

    public decimal Prizepool { get; set; }

    public DateOnly Date { get; set; }
    
    public GameStatus Status { get; set; }

    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();

    public virtual ICollection<Winner> Winners { get; set; } = new List<Winner>();
    
    public virtual ICollection<WinningNumbers> WinningNumbers { get; set; } = new List<WinningNumbers>();

}
