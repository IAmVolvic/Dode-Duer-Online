using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Board
{
    public Guid Id { get; set; }

    public Guid Userid { get; set; }

    public Guid Gameid { get; set; }

    public Guid Priceid { get; set; }

    public DateOnly Dateofpurchase { get; set; }

    public virtual ICollection<Chosennumber> Chosennumbers { get; set; } = new List<Chosennumber>();

    public virtual Game Game { get; set; } = null!;

    public virtual Price Price { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
