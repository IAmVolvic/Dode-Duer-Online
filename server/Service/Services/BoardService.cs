using System.Globalization;
using API.Exceptions;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services;

public class BoardService(IBoardRepository boardRepository, IPriceRepository priceRepository, IGameRepository gameRepository, IUserService userService, IGameService gameService) : IBoardService
{
    public BoardResponseDTO PlayBoard(PlayBoardDTO playBoardDTO)
    {
        var board = new Board();
        board.Userid = playBoardDTO.Userid;
        board.Dateofpurchase = playBoardDTO.Dateofpurchase;
        board.Gameid = gameRepository.GetActiveGame().Id;
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
    
    public List<WinnerResponseDTO> IdentifyWinners(Guid gameId, List<int> winningNumbers)
    {
        var boards = boardRepository.GetBoards().Where(b => b.Gameid == gameId).ToList();
        var winningBoards = boards.Where(b =>
        {
            var chosenNumbers = b.Chosennumbers.Select(n => n.Number ?? 0).ToList();
            var matchedNumbersCount = winningNumbers.Intersect(chosenNumbers).Count();
            return matchedNumbersCount == 3;
        }).ToList();
        
        var winners = winningBoards.Select(b =>
        {
            decimal wonAmount = b.Price.Price1;  
            return WinnerResponseDTO.FromBoard(b, wonAmount);
        }).ToList();

        return winners;
    }
    
    public List<WinnerResponseDTO> GetWinners(Guid gameId)
    {
        var winners = gameRepository.GetWinnersWithGame(gameId);
        return winners.Select(w => new WinnerResponseDTO
        {
            UserName = w.User.Name,
            //WonAmount = w.WonAmount,
            WeekNumber = ISOWeek.GetWeekOfYear(w.Game.Date.ToDateTime(new TimeOnly(0, 0)))
        }).ToList();
    }
}

