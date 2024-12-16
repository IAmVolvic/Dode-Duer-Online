using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IBoardRepository
{
    public Board PlayBoard(Board board);
    public List<Board> GetBoards();
    public List<Board> GetBoardsFromGame(Guid gameId);
}