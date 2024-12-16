using API.Exceptions;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services;

public class BoardService(IBoardRepository boardRepository, IPriceRepository priceRepository, IUserService userService, IGameService gameService) : IBoardService
{
    public BoardResponseDTO PlayBoard(PlayBoardDTO playBoardDTO)
    {
        var board = new Board();
        board.Userid = playBoardDTO.Userid;
        board.Dateofpurchase = playBoardDTO.Dateofpurchase;
        board.Gameid = gameService.GetActiveGame().Id;
        board.Id = Guid.NewGuid();
        board.Priceid = priceRepository.GetPrice(playBoardDTO.Numbers.Count).Id;
        board.Chosennumbers = playBoardDTO.Numbers.Select(n => new Chosennumber
            {
                Id = Guid.NewGuid(), 
                Boardid = board.Id, 
                Number = n 
            })
            .ToList();
        var price = priceRepository.GetPrice(playBoardDTO.Numbers.Count).Price1;
        var newBoard = boardRepository.PlayBoard(board);
        if (newBoard != null)
        {
            userService.UpdateUserBalance(price,newBoard.Userid);
            var prize = GetBoards().Sum(b => b.Price);
            gameService.UpdatePrizePool(prize);
            return new BoardResponseDTO().FromBoard(newBoard);
        }
        throw new ErrorException("BoardService","Board could not be played");
    }

    public List<BoardResponseDTO> GetBoards()
    {
        var boards = boardRepository.GetBoards();
        var boardsDto = boards.Select(b => new BoardResponseDTO().FromBoard(b)).ToList();
        return boardsDto;
    }

    public List<BoardGameResponseDTO> GetBoardsFromGame(Guid gameId)
    {
        var boards = boardRepository.GetBoardsFromGame(gameId);
        var boardsDto = boards.Select(b => new BoardGameResponseDTO().FromBoard(b)).ToList();
        return boardsDto;
    }

    public AutoplayBoardDTO AutoplayBoard(PlayAutoplayBoardDTO playAutoplayBoardDTO)
    {
        var board = new BoardAutoplay();
        board.Id = Guid.NewGuid();
        board.UserId = playAutoplayBoardDTO.Userid;
        board.LeftToPlay = playAutoplayBoardDTO.LeftToPlay;
        board.ChosenNumbersAutoplays = playAutoplayBoardDTO.Numbers.Select(n => new ChosenNumbersAutoplay()
            {
                Id = Guid.NewGuid(), 
                BoardId = board.Id, 
                Number = n 
            })
            .ToList();

        var newBoard = boardRepository.AutoplayBoard(board);
        return new AutoplayBoardDTO().FromBoard(newBoard);
    }

    public void PlayAllAutoplayBoards()
    {
        var boardsToPlay = boardRepository.GetAutoplayBoards();
        var adjustedBoards = AdjustLeftToPlay(boardsToPlay);
        

        var boards = adjustedBoards.Select(b => new PlayBoardDTO().fromAutoplay(b)).ToList();

        foreach (var board in boards)
        {
            var userBalance = userService.CheckUsersBalance(board.Userid);
          
            var price = priceRepository.GetPrice(board.Numbers.Count());

            if (userBalance < price.Price1)
            {
                PlayBoard(board);
            }
        }

        foreach (var board in adjustedBoards)
        {
            boardRepository.AdjustLeftToPlay(board);
        }
    }


    public List<BoardAutoplay> AdjustLeftToPlay(List<BoardAutoplay> boards)
    {
        var boardsLeft = new List<BoardAutoplay>();
        foreach (var board in boards)
        {
            if (board.LeftToPlay-1 <1)
            {
                boardRepository.DeleteBoardLeftToPlay(board);
            }
            else
            {
                boardsLeft.Add(boardRepository.AdjustLeftToPlay(board));
            }
        }

        return boardsLeft;
    }
}