namespace DataAccess.Models;

public class BoardAutoplay
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int LeftToPlay { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<ChosenNumbersAutoplay> ChosenNumbersAutoplays { get; set; } = new List<ChosenNumbersAutoplay>();
}