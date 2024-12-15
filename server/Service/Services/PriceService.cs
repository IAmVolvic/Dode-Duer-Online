using DataAccess.Interfaces;
using Service.Services.Interfaces;
using Service.TransferModels.Responses;

namespace Service.Services;

public class PriceService(IPriceRepository priceRepository) : IPriceService
{
    public List<PriceDto> GetPrices()
    {
        var prices = priceRepository.GetPrices();
        var pricesDto = prices.Select(p => new PriceDto().FromPrice(p)).ToList();
        return pricesDto;
    }
}