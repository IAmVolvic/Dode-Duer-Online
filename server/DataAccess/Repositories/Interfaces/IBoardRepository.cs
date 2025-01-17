﻿using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IBoardRepository
{
    public Board PlayBoard(Board board);
    public List<Board> GetBoards();
    public List<Board> GetBoardsFromGame(Guid gameId);
    public BoardAutoplay AutoplayBoard(BoardAutoplay board);
    public List<BoardAutoplay> GetAutoplayBoards();
    public BoardAutoplay AdjustLeftToPlay(BoardAutoplay board);
    public void DeleteBoardLeftToPlay(BoardAutoplay board);
    public List<BoardAutoplay> GetAutoplayBoards(Guid userId);
}