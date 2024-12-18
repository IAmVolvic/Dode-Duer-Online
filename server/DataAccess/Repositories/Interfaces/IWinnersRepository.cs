using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IWinnersRepository
{
    public void AddWinners(List<Winner> winners);
    public List<Winner> GetWinners(Guid gameId);
}