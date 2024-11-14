using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid Userid { get; set; }

    public decimal Amount { get; set; }

    public string Mobilepayid { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
