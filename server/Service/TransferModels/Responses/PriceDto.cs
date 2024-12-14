using DataAccess.Models;

namespace Service.TransferModels.Responses;

public class PriceDto
{
    public decimal Price1 { get; set; }

    public decimal Numbers { get; set; }

    public PriceDto FromPrice(Price price)
    {
        return new PriceDto()
        {
            Price1 = price.Price1,
            Numbers = price.Numbers
        };
    }
}