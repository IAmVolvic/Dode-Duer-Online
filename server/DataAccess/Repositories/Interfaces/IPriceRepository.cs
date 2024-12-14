using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IPriceRepository
{
    public List<Price> GetPrices();
    public Price GetPrice(int numbers);
}