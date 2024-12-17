namespace Service.TransferModels.Responses;

public class WinnersDto
{
    public Guid Gameid { get; set; }
    public string Name { get; set; }
    public Guid UserId { get; set; }
    public decimal Prize { get; set; } = 0;
    public int NumberOfWinningBoards { get; set; }
}