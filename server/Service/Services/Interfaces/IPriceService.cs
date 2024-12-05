using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface IPriceService
{
    public List<PriceDto> GetPrices();
}