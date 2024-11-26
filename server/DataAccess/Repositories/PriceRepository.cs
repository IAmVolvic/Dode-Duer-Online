using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Repositories;

public class PriceRepository(LotteryContext context) : IPriceRepository
{
    public Price GetPrice(int numbers)
    {
        var price = context.Prices.FirstOrDefault(p => p.Numbers == numbers);
        return price;
    }
}