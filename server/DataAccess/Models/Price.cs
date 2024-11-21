using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Price
{
    public Guid Id { get; set; }

    public decimal Price1 { get; set; }

    public decimal Numbers { get; set; }

    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();
}
