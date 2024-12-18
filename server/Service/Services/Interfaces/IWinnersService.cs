using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IWinnersService
{
    public List<WinnersDto> GetWinners(Guid gameId);
}