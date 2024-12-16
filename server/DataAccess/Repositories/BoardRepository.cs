using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class BoardRepository(LotteryContext context) : IBoardRepository
{
    public Board PlayBoard(Board board)
    {
        context.Boards.Add(board);
        context.SaveChanges();
        return board;
    }

    public List<Board> GetBoards()
    {
        return context.Boards.Include(b => b.Price).Include(b => b.Chosennumbers).ToList();
    }

    public List<Board> GetBoardsFromGame(Guid gameId)
    {
        return context.Boards.Include(b => b.Price).Include(b => b.Chosennumbers).Where(b => b.Gameid == gameId)
            .ToList();
    }
}