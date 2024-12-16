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
        return context.Boards.Include(b => b.User).Include(b => b.Chosennumbers).Where(b => b.Gameid == gameId)
            .ToList();
    }

    public BoardAutoplay AutoplayBoard(BoardAutoplay board)
    {
        context.BoardAutoplays.Add(board);
        context.SaveChanges();
        return board;
    }

    public List<BoardAutoplay> GetAutoplayBoards()
    {
        return context.BoardAutoplays.Include(b => b.ChosenNumbersAutoplays).ToList();
    }

    public BoardAutoplay AdjustLeftToPlay(BoardAutoplay board)
    {
        context.BoardAutoplays.Update(board);
        context.SaveChanges();
        return board;
    }

    public void DeleteBoardLeftToPlay(BoardAutoplay board)
    {
        context.BoardAutoplays.Remove(board);
        context.SaveChanges();
    }
}