using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Winner
{
    public Guid Id { get; set; }

    public Guid Gameid { get; set; }

    public Guid Userid { get; set; }

    public decimal Wonamount { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
