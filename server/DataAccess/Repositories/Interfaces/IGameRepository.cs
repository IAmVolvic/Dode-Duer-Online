﻿using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IGameRepository
{
    public Game NewGame(Game game, Game activeGame);
    public Game GetActiveGame();
    void AddWinningNumbers(List<WinningNumbers> winningNumbers);
    public void UpdatePrizePool(decimal newPrizePool);
    public List<Winner> GetWinnersWithGame();

}