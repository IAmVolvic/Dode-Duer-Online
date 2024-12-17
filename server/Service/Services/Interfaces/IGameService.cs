using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IGameService
{
    public GameResponseDTO NewGame(int prize);
    public GameResponseDTO NewGameFromMonday(int prize);
    public bool IsAnyGame();
    public Guid? GetActiveGameId();
   public WinningNumbersResponseDTO SetWinningNumbers(Guid gameId, int[] winningNumbers);
   public List<WinnerResponseDTO> GetWinners(Guid gameId);
   void UpdatePrizePool(decimal prize);
}