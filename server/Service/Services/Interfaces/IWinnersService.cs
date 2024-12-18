using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IWinnersService
{
    public void AddWinners(List<WinnersDto> winners);
}