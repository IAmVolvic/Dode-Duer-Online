using DataAccess.Interfaces;
using Service.Services.Interfaces;
using Service.TransferModels.Responses;

namespace Service.Services;

public class WinnersService(IWinnersRepository winnersRepository) : IWinnersService
{
    public void AddWinners(List<WinnersDto> winners)
    {
        winnersRepository.AddWinners(winners.Select(w => w.ToWinner()).ToList());
    }
}