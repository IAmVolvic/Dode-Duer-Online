using System.Text.Json.Serialization;
using DataAccess.Types.Enums;

namespace DataAccess.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phonenumber { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserEnrolled Enrolled { get; set; }
    
    public decimal Balance { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserStatus Status { get; set; }

    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();
    public virtual ICollection<BoardAutoplay> BoardAutoplays { get; set; } = new List<BoardAutoplay>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Winner> Winners { get; set; } = new List<Winner>();
}
