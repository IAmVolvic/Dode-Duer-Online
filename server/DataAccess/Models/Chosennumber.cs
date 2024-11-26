using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Chosennumber
{
    public Guid Id { get; set; }

    public Guid Boardid { get; set; }

    public int? Number { get; set; }

    public virtual Board Board { get; set; } = null!;
}
