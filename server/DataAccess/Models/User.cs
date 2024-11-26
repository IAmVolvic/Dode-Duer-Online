using DataAccess.Types.Enums;

namespace DataAccess.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phonenumber { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;
    
    public UserEnrolled Enrolled { get; set; }
    
    public decimal Balance { get; set; }
    
    public UserRole Role { get; set; }
    
    public UserStatus Status { get; set; }

    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Winner> Winners { get; set; } = new List<Winner>();
}
