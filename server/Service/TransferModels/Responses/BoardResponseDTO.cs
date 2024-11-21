namespace Service.TransferModels.Responses;

public class BoardResponseDTO
{
    public Guid Id { get; set; }

    public Guid Userid { get; set; }

    public Guid Gameid { get; set; }

    public decimal Price { get; set; }

    public DateOnly Dateofpurchase { get; set; }
}