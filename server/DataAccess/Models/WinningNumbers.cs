namespace DataAccess.Models;

public partial class WinningNumbers
{
    public Guid Id { get; set; }
    
    public Guid GameId { get; set; }
    
    public int Number { get; set; }

    public Game Game { get; set; }
}