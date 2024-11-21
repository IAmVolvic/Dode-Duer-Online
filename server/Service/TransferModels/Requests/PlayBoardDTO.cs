namespace Service.TransferModels.Requests;

public class PlayBoardDTO
{
    public Guid Userid { get; set; }
    public DateOnly Dateofpurchase { get; set; }
    public List<int> Numbers { get; set; }
}