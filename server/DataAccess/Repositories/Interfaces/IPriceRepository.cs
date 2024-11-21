using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IPriceRepository
{
    public Price GetPrice(int numbers);
}