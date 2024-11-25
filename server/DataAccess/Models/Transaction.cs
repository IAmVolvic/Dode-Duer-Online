using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    // New fields based on your updated table schema
    public string TransactionPhoneNumber { get; set; } = null!; // MobilePay phone number
    public string TransactionUsername { get; set; } = null!; // MobilePay Username
    public string TransactionNumber { get; set; } = null!; // MobilePay transaction number
    public string TransactionStatus { get; set; } = "Pending"; // Default status "Pending"

    // Navigation property for the related User
    public virtual User User { get; set; } = null!;
}
