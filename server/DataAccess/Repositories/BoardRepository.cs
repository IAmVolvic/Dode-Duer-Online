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
        return context.Boards.Include(b => b.Game).Include(b => b.User).Include(b => b.Price).Include(b => b.Chosennumbers).ToList();
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
        var trackedEntity = context.BoardAutoplays.Find(board.Id);
        if (trackedEntity != null)
        {
            trackedEntity.LeftToPlay = board.LeftToPlay;
            context.SaveChanges();
        }

        return trackedEntity;
    }

    public void DeleteBoardLeftToPlay(BoardAutoplay board)
    {
        // Ensure related entities are loaded
        context.Entry(board)
            .Collection(b => b.ChosenNumbersAutoplays)
            .Load();

        // Remove the parent entity
        context.BoardAutoplays.Remove(board);

        // Save changes
        context.SaveChanges();
    }

    public List<BoardAutoplay> GetAutoplayBoards(Guid userId)
    {
        return context.BoardAutoplays.Include(b=> b.ChosenNumbersAutoplays).Where(b => b.UserId == userId).ToList();
    }
}