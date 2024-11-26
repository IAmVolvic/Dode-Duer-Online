using System;
using System.Collections.Generic;
using System.Transactions;

namespace DataAccess.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid Userid { get; set; }

    public string Transactionnumber { get; set; } = null!;
    
    public TransactionStatus Transactionstatus { get; set; }

    public virtual User User { get; set; } = null!;
}
