using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IGameService
{
    public GameResponseDTO NewGame(int prize);
}