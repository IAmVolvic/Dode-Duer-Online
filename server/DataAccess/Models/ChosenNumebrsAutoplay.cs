namespace DataAccess.Models;

public class ChosenNumbersAutoplay
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public int Number { get; set; }

    public virtual BoardAutoplay BoardAutoplay { get; set; }
}