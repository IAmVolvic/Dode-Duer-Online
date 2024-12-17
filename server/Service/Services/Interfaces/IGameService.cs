using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IGameService
{
    public GameResponseDTO NewGame(int prize);
    public GameResponseDTO NewGameFromMonday(int prize);
    public bool IsAnyGame();
    
    public WinningNumbersResponseDTO SetWinningNumbers(WinningNumbersRequestDTO data);
    
    public void UpdatePrizePool(decimal newPrizePool);
    public List<GameResponseDTO> GetAllGames();
}